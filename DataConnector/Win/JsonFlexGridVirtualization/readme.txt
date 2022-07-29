JsonFlexGridVirtualization sample for Winforms
------------------------------------------
Shows how to use the Json Connector to query a large Json object and output the values in C1FlexGrid. 

The sample shows how to create connection string, connect to data source, perform query, then bind data to C1FlexGrid. DataCollection is used 
in FlexGrid with Virtualization to output the data. The ISupportIncrementalLoading interface is implemented so newer items are loaded dynamically
while scrolling.