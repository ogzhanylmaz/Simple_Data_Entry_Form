CREATE TABLE [dbo].[vpn_users] (
    [Id]          INT NOT NULL DEFAULT 0,
    [username]    VARCHAR (50) NOT NULL,
    [password]    VARCHAR (50) NOT NULL,
    [firstname]   VARCHAR (50) NULL,
    [surname]     VARCHAR (50) NULL,
    [recorded_by] VARCHAR (50) NULL
);

