using System;
using System.Collections.Generic;
using System.IO;

namespace TC1K_LaserMonitor
{
    
    public class SettingsFileHandler
    {

        public Reporter Rep;
        public bool checkFieldDiscrepancies = false;
        public bool makeUpdatedFileOnDiscrepancies = true;
        public string errString;
        public string checkedOffString = "thisOneHasBeenFilled";

        public SettingsFileHandler()
        {        
        }


        public Object loadSettingsFile(out bool loadedOK, string settingsFilePath, optionSettings dataObject)
        {

            Type dataObjType = dataObject.GetType();

            String[] fieldsMissingFromDataObject = new String[0]; // fields that are in the data object, but are NOT in the settings file
            List<String> updatedSettingsFile = new List<string>();
            // initialize 'fieldsMissingFromSettingsFile with *all the props of the data object, then remove them as we set them
            var allDataObjectPropObjs = dataObjType.GetProperties();
            String[] fieldsMissingFromSettingsFile = new String[allDataObjectPropObjs.Length]; // fields that are in the settings file, but are NOT in the data object
            for (int ii = 0; ii < allDataObjectPropObjs.Length; ii++)
            {
                String thisPropName = allDataObjectPropObjs[ii].Name;
                fieldsMissingFromSettingsFile[ii] = thisPropName;
            }

            string csvFieldName = null;
            string csvFieldValString = null;
            try
            {
                // go through the csv file one line at a time
                using (System.IO.StreamReader SR = new System.IO.StreamReader(settingsFilePath))
                {
                    string line;
                    while ((line = SR.ReadLine()) != null)
                    {
                        // reset to avoid giving false error messages
                        csvFieldName = null;
                        csvFieldValString = null;

                        String fullLineCommentPrefix = "//";
                        if ((line == "") || line.StartsWith(fullLineCommentPrefix) || line.StartsWith(","))
                        {
                            updatedSettingsFile.Add(line);
                            continue;
                        }
                        string[] Splitline = line.Split(',');
                        csvFieldName = Splitline[0];
                        csvFieldValString = Splitline[1];
                        //string[] itemCommentArray = new string[Splitline.Length-2]; // there could be multiple, separated by commas
                        //string itemComment = String.Join(",", itemCommentArray);
                        System.Reflection.PropertyInfo propInfo = dataObjType.GetProperty(csvFieldName);
                        if (propInfo == null) // if the csv file has a field that does NOT correspond to a property of the dataObject
                        {
                            errString = string.Format("Option settings file contains an unknown field: {0}", csvFieldName);
                            Rep.Post(errString,repLevel.error,null);
                            //fieldsMissingFromDataObject.Add(csvFieldName); // doesn't work in this version of C#
                            continue;
                        }
                        else
                        {
                            updatedSettingsFile.Add(line);
                        }


                        int whereToCheckOff = Array.IndexOf(fieldsMissingFromSettingsFile, csvFieldName);
                        fieldsMissingFromSettingsFile[whereToCheckOff] = checkedOffString;




                        var csvFieldVal = new Object();
                        Type propertyType = propInfo.PropertyType;
                        if (propertyType.IsEnum)
                        {
                            csvFieldVal = Enum.Parse(propertyType, csvFieldValString);
                        }
                        else
                        {
                            csvFieldVal = Convert.ChangeType(csvFieldValString, propertyType);
                        }
                        propInfo.SetValue(dataObject, csvFieldVal, null);
                    }

                    ////fieldsMissingFromSettingsFile.Remove(csvFieldName); // delete them as you set them // doesn't work in this version of C#
                    // so I'm doing this instead, at the end
                    foreach (string oneString in fieldsMissingFromSettingsFile)
                    {
                        if (oneString != checkedOffString)
                        {
                            errString = string.Format("Option settings file is missing a necessary field: {0}", oneString);
                            Rep.Post(errString, repLevel.error, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string exceptionMsg;
                if (csvFieldName==null)
                {
                    exceptionMsg = String.Format("Exception while reading settings file {0}", settingsFilePath);
                }
                else
                {
                    exceptionMsg = String.Format("Exception in field {0}, value {1} while reading settings file {2}", csvFieldName, csvFieldValString, settingsFilePath);
                }
                Rep.Post(exceptionMsg, repLevel.error, ex.Message);
                loadedOK = false;
                return (dataObject);
            }

            bool anyMissingFromDataObject;
            bool anyMissingFromSettingsFile = false;
            if (checkFieldDiscrepancies)
            { 
                // announce any missing fields
                anyMissingFromDataObject = fieldsMissingFromDataObject.Length > 0;
                anyMissingFromSettingsFile = fieldsMissingFromSettingsFile.Length > 0;

                String netString = "Inconsistent fields while loading " + System.IO.Path.GetFileName(settingsFilePath) + "!" +
                    Environment.NewLine + Environment.NewLine;
                if (anyMissingFromDataObject)
                {
                    String announceMissingFromObject = "These fields appear in the settings file but not in the data object:" + Environment.NewLine;
                    String missingFromObjectString = String.Join(Environment.NewLine, fieldsMissingFromDataObject);
                    netString = String.Concat(netString, announceMissingFromObject, missingFromObjectString, Environment.NewLine, Environment.NewLine);
                }
                if (anyMissingFromSettingsFile)
                {
                    String announceMissingFromFile = "These fields appear in the data object but not in the settings file:" + Environment.NewLine;
                    String missingFromFileString = String.Join(Environment.NewLine, fieldsMissingFromSettingsFile);
                    netString = String.Concat(netString, announceMissingFromFile, missingFromFileString, Environment.NewLine, Environment.NewLine);
                    for (int ind=0; ind < fieldsMissingFromSettingsFile.Length; ind++)
                    {
                        string updatedLine = fieldsMissingFromSettingsFile[ind] +  ",ADD_NEW_VALUE_HERE";
                        updatedSettingsFile.Add(updatedLine);
                    }
                }
                if (anyMissingFromDataObject || anyMissingFromSettingsFile)
                {
                    if (makeUpdatedFileOnDiscrepancies)
                    {
                        netString = String.Concat(netString, Environment.NewLine, "An updated settings file has been added to the directory.");
                        string updatedFileName = settingsFilePath.Substring(0,settingsFilePath.Length-4) + "_updated.csv";
                        File.WriteAllLines(updatedFileName, updatedSettingsFile.ToArray());
                    }
                    System.Windows.Forms.MessageBox.Show(netString);
                }
            }
            loadedOK = !anyMissingFromSettingsFile;
            return (dataObject);

        }


        public void saveSettingsFile(optionSettings dataObject, string settingsFilePath)
        {
            FileInfo fileInfo = new FileInfo(settingsFilePath);
            StreamWriter writer = fileInfo.CreateText();
            var propertyArray = dataObject.GetType().GetProperties();
            for (int ii = 0; ii < propertyArray.Length; ii++)
            {
                String propertyString = propertyArray[ii].Name;
                String valueString;
                try
                {
                    valueString = propertyArray[ii].GetValue(dataObject,null).ToString();
                }
                catch
                {
                    valueString = "";
                }
                writer.WriteLine(propertyString + "," + valueString);
            }
            writer.Close();
        }





    }

}
