ExtractErrorLogs
-------------------------------------------------------------------------------
This is a console app sample to demonstrate the capabilities of the C1TextParser library - TemplateBased extractor

From a server log file, extract all the ERROR logs.
Each log follows a predefined fixed structure, that consists in 4 major elements.
These are: The date, the time (up to ms), the log type and finally, the description of the log.
The input stream content, the template and also the extracted result (in Json format) are displayed in the console.
Also, the extracted result can be visualized at "ExtractErrorLogs.csv" in the current working directory.