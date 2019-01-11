# Google Speak PSI
This is a PSI component to translate text to audio using a Google Cloud API.

# Setup
This component uses the google text to speech api which requires an api key. This key is accessed through an environment variable.
It also requires a nuget package for the google text to speech api client.

To set up your credentials you can follow the instructions [here](https://cloud.google.com/docs/authentication/production)

Generate a JSON key file for your api key using the google cloud platform. Then set the environment variable GOOGLE_APPLICATION_CREDENTIALS
to the path of the file.

# Nuget Packages You Might Forget
Microsoft.Psi.Runtime   
Microsoft.Psi.Runtime.Windows   
Microsoft.Psi.Audio   
Google.Cloud.TextToSpeech.V1   

# Contributors 
Edgar Sosa
Alex Mussell
James Brooks