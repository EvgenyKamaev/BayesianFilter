CREATE TABLE [BayesianFilter].[dbo].[Bayesian] (
    Id int IDENTITY(1,1) PRIMARY KEY,
	Subject nvarchar(64) NOT NULL,
    CreatedDate datetime NOT NULL,
    IsSpam bit NOT NULL,
);
--drop table [BayesianFilter].[dbo].[Bayesian]

CREATE TABLE [BayesianFilter].[dbo].[Exceptions] (
    Id int IDENTITY(1,1) PRIMARY KEY,
	Subject nvarchar(64) NOT NULL,
    CreatedDate datetime NOT NULL
);
--drop table [BayesianFilter].[dbo].[Exceptions]