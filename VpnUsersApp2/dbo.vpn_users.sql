CREATE TABLE [dbo].[vpn_users] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [username]    VARCHAR (50) NOT NULL,
    [password]    VARCHAR (50) NOT NULL,
    [firstname]   VARCHAR (50) NULL,
    [surname]     VARCHAR (50) NULL,
    [recorded_by] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

