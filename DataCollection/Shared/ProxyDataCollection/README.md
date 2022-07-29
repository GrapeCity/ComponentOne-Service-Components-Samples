## ProxyDataCollection
#### [Download as zip](https://grapecity.github.io/DownGit/#/home?url=https://github.com/GrapeCity/ComponentOne-Service-Components-Samples/tree/master/DataCollection/Shared/ProxyDataCollection)
____
#### Shows how to use C1ProxyDataCollection in various platforms WPF, WinForms and Blazor WASM.
____
The project ProxyDataCollection.Server has the FinancialHub that provides the data, and it is mandatory to run it no matter what client platform is used.
The server takes a list of stock symbols and generate random data that is continually updated, the hub notifies all the clients of this changes and the C1ProxyDataCollection updates internally and notify the controls through the standard CollectionChanged event from INotifyCollectionChanged. 
To run all the platforms together is possible to edit solution properties to set "Multiple startup projects" and select more than one client.
