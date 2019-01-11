# Psi Components
This repo contains a collection of Microsoft PSI components developed by the SLIM research group.

# Components
| Component Name  | Description                                                                             | Type     |
|-----------------|-----------------------------------------------------------------------------------------|----------|
| GoogleASR       | Speech to text using Google API                                                         | Operator |
| GoogleTranslate | Text language translation using Google API                                              | Operator |
| GoogleSpeak     | Text to speech using Google API                                                         | Operator |
| AudioOutput     | Plays PSI Audio data through speaker                                                    | Consumer |
| TobiiEyeTracker | Aquires sensor data from Tobii eye tracker                                              | Producer |
| ActiveMQ        | Sends and receives strings with timestamps through ActiveMQ                             | Bridge   |
| DataFaucet      | Data gatekeeper for component to control whether or not the component will receive data | Control  |
| AggregateDump   | Allows aggregating and triggered dumping of Audio bytes                                 | Control  ||

* Operators: Use an input data stream that they operate on to create an output data stream.   
* Consumers: Use an input data stream but do not have an output stream.   
* Producers: Produce an input data stream but do not have any input PSI data streams.   
* Bridges: Provide a message passing interface between PSI components and programs written in other programming languages.   
* Controls: Provide an interface to control pipeline data flow

# Examples
SpeechTranslate.cs uses GoogleASR, GoogleTranslate, GoogleSpeak, ActiveMQ, AudioOutput, DataFaucet, and AggregateDump. 
It launches a GUI to translate audio from one language to another. Just launch the program. Click the speak button
speak into your computer and then press stop. Note: this example has only been tested on windows. 

In order to use the example you will need to do the setup required for each component listed above. The setup 
instructions for each component are in the READMEs in each components folder. You may also need to add System.Drawing
as a reference to compile the GUI.

# Contributors
Alex Mussell   
Heather Larsen   
James Brooks   
Edgar Sosa   