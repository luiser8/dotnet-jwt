CREATE TABLE Users
(
     Id INT IDENTITY(1,1) NOT NULL ,
     FirstName VARCHAR(155) NOT NULL  ,
     LastName VARCHAR(155) NOT NULL  ,
     Email VARCHAR(155) NOT NULL UNIQUE ,
     UserName VARCHAR(155) NOT NULL UNIQUE ,
     PasswordHash NTEXT NOT NULL  ,
     PasswordSalt NTEXT NOT NULL  ,
     AccessToken NTEXT NOT NULL  ,
     RefreshToken NTEXT NOT NULL  ,
     TokenCreated DATETIME NOT NULL ,
     TokenExpires DATETIME NOT NULL ,
     CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (Id ASC) ON [PRIMARY]
)
GO