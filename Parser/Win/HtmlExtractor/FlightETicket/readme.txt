FlightETicket
-----------------------------------------------------------------------------------------------------------
This is a console app sample to demonstrate the fixed place holder extraction capabilities of the C1TextParser library - Html extractor

From a vietjetair e-ticket extract relevant information about the flight. 
Note that the email used as extraction source was modified on purpose (added random text at different locations) with the intent to show that html extractor is flexible enough to retrieve the intended text.
This consists on seven fixed place holders. These are: the passenger name, the booking number, the booking status, the fare type, the total amount, the city of departure and, finally, the year of booking.
The vietjetair email used as the extraction source is "vietjetairEmail2.html" and can be consulted in the current working directory. Also, "FlightETicket.csv" contains the parsing result.