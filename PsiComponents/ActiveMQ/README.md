# ActiveMQ Psi
This PSI component provides an interface to send messages using ActiveMQ.

# Setup
This component requires that an ActiveMQ executable exists on the system. The system must also be Windows.

This component uses JsonConfig for configurating. You need to include a default.conf file. The default.conf
file is a txt file that contains a json object. Meaning the file starst with { and ends with } .
To use this componet, add a section inside the curly braces in the default.conf that includes a section
similar to this:
	ActiveMQ : {
		Host : "localhost",
		Port : 61616,
		Protocol : "activemq:tcp",
		Executable : "C:\\Users\\AlexMussell\\Downloads\\apache-activemq-5.15.7-bin\\apache-activemq-5.15.7\\bin\\activemq"
	}	

Change the execuable path to the correct path on your computer. (Note: use two backslashed to seperate directories).

Make sure that your default.conf has been added as an [embedded resource](https://stackoverflow.com/a/39368856/7725203) when compiling/running your program.



# Using the Component (code)
Make sure that you have followed the setup steps.

You can construct several instances of the component. An instance of the component will pass messages 
from the pipeline to an ActiveMQ message queue, and feed messages into the pipeline (out of the component) 
from listening to an ActiveMQ message queue. Messages include a string and DateTime, both of which will be
encoded (into a json object with the fields "Content" and "DateTime") when they are sent to a message queue.
The json encoding is then wrapped in xml in an xml string (Due to the ActiveMQ C# libraries I could not avoid
the xml).

This component only allows strings to be sent as content. Other types of messages must be encoded into a string.

# Nuget Packages You Might Forget
JsonConfig
Microsoft.Psi.Runtime
Microsoft.Psi.Runtime.Windows
Apache.NMS
Apache.NMS.ActiveMQ
Apache.NMS.Stomp
Newtonsoft.Json
Newtonsoft.Json.Schema