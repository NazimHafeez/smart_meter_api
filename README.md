# smart_meter_api
Application to manage smart meter readings for customers

## Requirements

### As an Energy Company Account Manager, I want to be able to load a CSV file of Customer Meter Readings so that we can monitor their energy consumption and charge them accordingly.
### Create the following endpoint: 
	POST => /meter-reading-uploads 
### The endpoint should be able to process a CSV of meter readings. An example CSV file has been provided (Meter_reading.csv) 
### Each entry in the CSV should be validated and if valid, stored in a DB. 
### After processing, the number of successful/failed readings should be returned. 
### You should not be able to load the same entry twice 
### A meter reading must be associated with an Account ID to be deemed valid 
### Reading values should be in the format NNNNN 

### OPTIONAL: When an account has an existing read, ensure the new read isn’t older than the existing read








