CREATE TABLE [dbo].[vpn_users]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY DEFAULT 0, 
    [username] VARCHAR(50) NOT NULL, 
    [password] VARCHAR(50) NOT NULL, 
    [firstname] VARCHAR(50) NULL, 
    [surname] VARCHAR(50) NULL, 
    [recorded_by] VARCHAR(50) NULL
)
