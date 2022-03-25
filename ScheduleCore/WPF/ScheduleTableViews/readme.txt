ScheduleTableViews 
------------------------------------------
Demonstrates how to use C1ScheduleStorage component in a scheduling application.

The sample application includes C1.WPF.ScheduleTableViews assembly which contains table view, agenda view and simple dialog for creating and editing appointments. 
Table and Agenda views allows you to represent data form the C1ScheduleStorage component in compact form aceptable for quick search and group operations like removing appointments.
These views are separate controls, so you can use them in any place of your application.
Both views require reference to the C1ScheduleStorage component filled with data. 
They can only show Appointments which are loaded into the owning C1ScheduleStorage component.

The C1.WPF.ScheduleTableViews library contains 2 controls that you can use to extend your scheduling app. 
The AgendaView control represents appointments grouped by the start date. 
You can set this view to show agenda for the current day, next 7 days or for the whole interval of days represented by the C1ScheduleStorage component.
The main goal of AgendaView is to show nearest events in compact form, so that end-user can quickly pick some event without navigating via all events.

The TableView control represents all appointments in a table view which supports sorting, filtering, in-place editing and removing appointments by end-user.
This view can be customized by changing properties of AppointmentField objects. This allows to show or hide appointment fields, setup grouping or sort options.
Set TableView.Active property to true to filter out inactive appointments from this view.

Double click on eny appointment in the AgendaView or TableView opens EditAppointmentDialog for editing.
All end-user changes are propagated from the C1ScheduleStorage component to table views and vice versa with no custom code.

Both AgendaView and TableView controls are inherited from the C1.WPF.Grid.FlexGrid control. 
That allows you to customize appearance and behavior based on FlexGrid's object model.

The sample application uses new C1.WPF.Ribbon.C1Ribbon control with tabs allowing to change table view settings, create or delete appointments, or import appointments from other sources.






