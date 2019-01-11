# Google Translate PSI
This is a PSI component used to translate text in one language to text in another language using the Google Cloud Translation API.

# Setup
This component uses the google translate api which requires an api key. This key is accessed through an environment variable.
It also requires a nuget package for the google translate api client.

To set up your credentials you can follow the instructions [here](https://cloud.google.com/docs/authentication/production)

Generate a JSON key file for your api key using the google cloud platform. Then set the environment variable GOOGLE_APPLICATION_CREDENTIALS
to the path of the file.

# Using the Component
Connect this component to another PSI component that is outputing strings in the input langauge that is specified
when instantiating this component.

# Nuget Packages You Might Forget
Google.Cloud.Translation.V2   
Microsoft.Psi.Runtime   
Microsoft.Psi.Runtime.Windows   

# Contributors
James Brooks
Alex Mussell