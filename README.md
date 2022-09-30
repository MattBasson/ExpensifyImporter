# ExpensifyImporter

A small application to imort Expensify receipts from Expensify

## Introduction

Working as a freelancer in the past and having all my receipts stored in a third party service called Expensify, without a facility 
too download the Image assets of the receipts that I have stored in this service. I want to shutdown the subscription because I was retiring my limited 
company and the services it depended on. This is what prompted me to create this tool that 
imports the data from a report generated via their report generator and store in a locally setup MySQL database.
So I can retain the receipt images for HMRC and not keep paying expensify to hold them for me.

## Data

There is no joy without the data, so the primary goal of this was to get the images of the receipts that I have uploaded.
Expensify, who I used to store my receipts, didnt offer the download of these images, only the download of their urls.
The url initially found was wrapper page inside their portal to which you would have to click another link to see the original image.

A lot of the inspiration for this mini project was found [here](https://community.expensify.com/discussion/3531/how-do-i-export-download-actual-images-of-receipts-for-document-support-storage)

This page described the woes of other individuals in a similar situation.

For the sake of getting the right report data I found the following by "*tullywork*" in the above link get to get the report data export right.

>1) From the Expenses Section in the App; select a receipt (doesnt matter which at this point)
>2) In the top right of the UI select Export To and select 'create new CSV layout'
>3) In the new layout, customize as desired, created a new column with the Formula: `{expense:receipt:url:direct}` then Save Export Format
>4) To get the expense id use this `{expense:id}`
>5) To get the Receipt id use this `{expense:receipt:id}`


Once you have the report created you can generate a report from your expensify dashboard and your relevant filters and the export will include
the direct urls to your expense imagery.
I chose the export format .xls (Excel)

I also did notice that expensify had its own Api for integrating with, found [here](https://integrations.expensify.com/Integration-Server/doc/#report-exporter).
I did look into this but thought that a simpler approach will be more suitable for how little I would be using this in the future.

### Reporting Dates
2017-02-01 - 2019-02-01

2019-02-01 - 2021-02-01

2021-02-01 - 2022-09-01

## Version 1 (starting small)

So i have the data with Urls pointing directly to the images I want along with expense data stored in Excel format.
I will most likely get more of these files over time and wish that they be automatically updated into my database.

The first iteration of this will be to write a console application that watches a director for file creates that are of type
.xlsx and read this into a database.

I will be using .net 6, entityFramework and Mysql as my stack.

### Code First
---
To create the model in your configured database .

#### Config

##### Database
update the configuration in the appsettings.json file in the the root of the  ExpensifyImporter.Database application.

see here an example

`  "MySql": {
    "Server": "localhost",
    "Port": "3306",    "Database": "expensify"
   }
`

User name and password have been ommitted from config so as not to commit this to version control.

##### User Secrets

Ensure to set user secrets for the following in database so that username and password are set when running.

If user secrets are not enabled, enable with this

`dotnet user-secrets init`     

after wich you need to add user secrets for username and password as follows insidee the database project  directory

` dotnet user-secrets set "MySql:User" "<username>"
` 

and

` dotnet user-secrets set "MySql:Password" "<password>"
` 

more on user secrets can be found [here](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows)

#### Entity Framework

##### Migrations

I make use of the dotnet ef tool to interact with EntityFramework for code first operations.

###### Setup

To set this up I run (assuming you have the dotnet-cli installed)

`dotnet tool install --global dotnet-ef`

Or to update

`dotnet tool update --global dotnet-ef `


###### Adding
To add a migration to the model the following can be used, from within the database project.

` dotnet ef migrations add <name of migration>`

###### Publishing

To publish you model to your configured database you can  run the following from within the database project.

`dotnet ef database update `


### Watching for file changes
---
For  this feature I will be using what has been documented [here](https://docs.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=net-6.0)

The parameters for which will be passed in through args at startup.


### Polling for file changes
---
Feature for checking the directory on a periodic basis and processing all documents



### Reading Excel data
---
For reading Excel data, I want to take advantage of the OpenXml standard and the nuget package OpenXml to acheive reading
the excel files read in by the file watcher and poller.


### Downloading Imagery
---
I hope to new up a httpClient for the sake of downloading the image via get request converting it to a byte array and storing in the database.

### Database 
---

Once all expense data is save including images, it gives me scope to create a light web portal to query my own dataset in the future.

#### Model

I wish to follow a code first approach to creating a model. 


#### Store in the cloud.

This will eventually be store in a datastore in the cloud, so it can be used for future projects.
