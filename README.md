# SAPKantar

This is a SAP integrated steelyard implementation for one of my clients. 

Cool part of this application is; an ActiveX object is embedded into SAP GUI. The ActiveX reads the weight from the steelyard over the COM port directly, and passes it to an ABAP text field. Therefore, the user doesn't have to deal with any external app to read the COM port.

For more info on how to embed an ActiveX into SAP GUI, check the sibling project [DaTag](https://github.com/keremkoseoglu/DaTag) which features a demo video + tutorial.