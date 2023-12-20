/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Message]
      ,[MessageTemplate]
      ,[Level]
      ,[TimeStamp]
      ,[Exception]
      ,[Properties]
  FROM [DotNet8AuthDb].[dbo].[Logs]
  WHERE Level = 'Error'
  Order by TimeStamp desc