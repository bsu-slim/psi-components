# Google ASR PSI
This is a PSI component used to translate audio into text using a Google Cloud API.

# Setup
This component uses the google speech api which requires an api key. This key is accessed through an environment variable.
It also requires a nuget package for the google speech api client.

To set up your credentials you can follow the instructions [here](https://cloud.google.com/speech-to-text/docs/reference/libraries)

Generate a JSON key file for your api key using the google cloud platform. Then set the environment variable GOOGLE_APPLICATION_CREDENTIALS
to the path of the file.

# Using this Component (code)
Connect this component to a PSI stream that is outputing AudioBuffers. Each AudioBuffer's audio content will be sent
individually to the Google Cloud Speech API.

# Nuget Package You Might Forget
Google.Cloud.Speech.V1   
Microsoft.Psi.Runtime   
Microsoft.Psi.Runtime.Windows   
Microsoft.Psi.Audio   

# Contributors
Alex Mussell
James Brooks