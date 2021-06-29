USE [master]
GO
/****** Object:  Database [SampleLoginDb]    Script Date: 6/29/2021 1:53:42 PM ******/
CREATE DATABASE [SampleLoginDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SampleLoginDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SampleLoginDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SampleLoginDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SampleLoginDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SampleLoginDb] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SampleLoginDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SampleLoginDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SampleLoginDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SampleLoginDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SampleLoginDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SampleLoginDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [SampleLoginDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SampleLoginDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SampleLoginDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SampleLoginDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SampleLoginDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SampleLoginDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SampleLoginDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SampleLoginDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SampleLoginDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SampleLoginDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SampleLoginDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SampleLoginDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SampleLoginDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SampleLoginDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SampleLoginDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SampleLoginDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SampleLoginDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SampleLoginDb] SET RECOVERY FULL 
GO
ALTER DATABASE [SampleLoginDb] SET  MULTI_USER 
GO
ALTER DATABASE [SampleLoginDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SampleLoginDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SampleLoginDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SampleLoginDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SampleLoginDb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'SampleLoginDb', N'ON'
GO
ALTER DATABASE [SampleLoginDb] SET QUERY_STORE = OFF
GO
USE [SampleLoginDb]
GO
/****** Object:  Table [dbo].[UserActivation]    Script Date: 6/29/2021 1:53:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserActivation](
	[UserId] [int] NOT NULL,
	[ActivationCode] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserActivation] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 6/29/2021 1:53:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](30) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Insert_User]    Script Date: 6/29/2021 1:53:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Insert_User 'test', 'test', 'test@test.com'
CREATE PROCEDURE [dbo].[Insert_User]
	@Username NVARCHAR(20),
	@Password NVARCHAR(20),
	@Email NVARCHAR(30)
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT UserId FROM Users WHERE Username = @Username)
	BEGIN
		SELECT -1 AS UserId -- Username exists.
	END
	ELSE IF EXISTS(SELECT UserId FROM Users WHERE Email = @Email)
	BEGIN
		SELECT -2 AS UserId -- Email exists.
	END
	ELSE
	BEGIN
		INSERT INTO [Users]
			   ([Username]
			   ,[Password]
			   ,[Email]
			   ,[CreatedDate])
		VALUES
			   (@Username
			   ,@Password
			   ,@Email
			   ,GETDATE())
		
		SELECT SCOPE_IDENTITY() AS UserId -- UserId			   
     END
END

GO
/****** Object:  StoredProcedure [dbo].[Validate_User]    Script Date: 6/29/2021 1:53:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Validate_User]
      @Username NVARCHAR(20),
      @Password NVARCHAR(20)
AS
BEGIN
      SET NOCOUNT ON;
      DECLARE @UserId INT, @LastLoginDate DATETIME
     
      SELECT @UserId = UserId, @LastLoginDate = LastLoginDate
      FROM Users WHERE Username = @Username AND [Password] = @Password
     
      IF @UserId IS NOT NULL
      BEGIN
            IF NOT EXISTS(SELECT UserId FROM UserActivation WHERE UserId = @UserId)
            BEGIN
                  UPDATE Users
                  SET LastLoginDate = GETDATE()
                  WHERE UserId = @UserId
                  SELECT @UserId [UserId] -- User Valid
            END
            ELSE
            BEGIN
                  SELECT -2 -- User not activated.
            END
      END
      ELSE
      BEGIN
            SELECT -1 -- User invalid.
      END
END

GO
USE [master]
GO
ALTER DATABASE [SampleLoginDb] SET  READ_WRITE 
GO
