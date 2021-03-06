USE [master]
GO
/****** Object:  Database [PersianAdminPanel]    Script Date: 7/30/2021 12:11:14 AM ******/
CREATE DATABASE [PersianAdminPanel]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PersianAdminPanel', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\PersianAdminPanel.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PersianAdminPanel_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\PersianAdminPanel_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [PersianAdminPanel] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PersianAdminPanel].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PersianAdminPanel] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET ARITHABORT OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PersianAdminPanel] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PersianAdminPanel] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PersianAdminPanel] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PersianAdminPanel] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET RECOVERY FULL 
GO
ALTER DATABASE [PersianAdminPanel] SET  MULTI_USER 
GO
ALTER DATABASE [PersianAdminPanel] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PersianAdminPanel] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PersianAdminPanel] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PersianAdminPanel] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PersianAdminPanel] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PersianAdminPanel', N'ON'
GO
ALTER DATABASE [PersianAdminPanel] SET QUERY_STORE = OFF
GO
USE [PersianAdminPanel]
GO
/****** Object:  Table [dbo].[tblLog]    Script Date: 7/30/2021 12:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[LogLevel] [nvarchar](50) NOT NULL,
	[Callsite] [nvarchar](max) NULL,
	[Duration] [bigint] NULL,
	[Exception] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRole]    Script Date: 7/30/2021 12:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRole](
	[Id] [bigint] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_tblRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSetting]    Script Date: 7/30/2021 12:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSetting](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[title] [varchar](100) NOT NULL,
	[value] [nvarchar](max) NOT NULL,
	[valueType] [varchar](100) NOT NULL,
 CONSTRAINT [PK_tblSetting] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserActivation]    Script Date: 7/30/2021 12:11:14 AM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 7/30/2021 12:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](1000) NOT NULL,
	[Email] [nvarchar](30) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NULL,
	[FK_User_tblRole] [bigint] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tblRole] ([Id], [Title]) VALUES (1, N'SuperAdmin')
INSERT [dbo].[tblRole] ([Id], [Title]) VALUES (2, N'Admin')
INSERT [dbo].[tblRole] ([Id], [Title]) VALUES (3, N'User')
SET IDENTITY_INSERT [dbo].[tblSetting] ON 

INSERT [dbo].[tblSetting] ([id], [title], [value], [valueType]) VALUES (1, N'initialized', N'1', N'bit')
SET IDENTITY_INSERT [dbo].[tblSetting] OFF
INSERT [dbo].[UserActivation] ([UserId], [ActivationCode]) VALUES (4, N'21fb6422-af92-488f-a34b-5212d4fb0032')
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [Username], [Password], [Email], [CreatedDate], [LastLoginDate], [FK_User_tblRole]) VALUES (4, N'admin', N'098f6bcd4621d373cade4e832627b4f6', N'admin@test.test', CAST(N'2021-06-29T18:52:23.200' AS DateTime), CAST(N'2021-07-30T00:07:36.610' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblSetting_title]    Script Date: 7/30/2021 12:11:14 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tblSetting_title] ON [dbo].[tblSetting]
(
	[title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_uq_Users_username]    Script Date: 7/30/2021 12:11:14 AM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [IX_uq_Users_username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblLog] ADD  CONSTRAINT [DF_tblLog_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[UserActivation]  WITH CHECK ADD  CONSTRAINT [FK_UserActivation_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserActivation] CHECK CONSTRAINT [FK_UserActivation_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_tblRole] FOREIGN KEY([FK_User_tblRole])
REFERENCES [dbo].[tblRole] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_tblRole]
GO
/****** Object:  StoredProcedure [dbo].[Insert_User]    Script Date: 7/30/2021 12:11:15 AM ******/
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
    IF EXISTS (SELECT UserId FROM Users WHERE Username = @Username)
    BEGIN
        SELECT -1 AS UserId -- Username exists.
    END
    ELSE IF EXISTS (SELECT UserId FROM Users WHERE Email = @Email)
    BEGIN
        SELECT -2 AS UserId -- Email exists.
    END
    ELSE
    BEGIN
        INSERT INTO [Users]
        (
            [Username],
            [Password],
            [Email],
            [CreatedDate],
            [FK_User_tblRole]
        )
        VALUES
        (@Username, @Password, @Email, GETDATE(), 3)

        SELECT SCOPE_IDENTITY() AS UserId -- UserId			   
    END
END
GO
/****** Object:  StoredProcedure [dbo].[spoc_initialize_db_data]    Script Date: 7/30/2021 12:11:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spoc_initialize_db_data]
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[tblSetting]
        WHERE [dbo].[tblSetting].[title] like 'initialized'
              AND [dbo].[tblSetting].[title] like '1'
    )
    BEGIN

        --[tblRole]
        INSERT INTO [dbo].[tblRole]
        (
            [Id],
            [Title]
        )
        VALUES
        (1, 'SuperAdmin'),
        (2, 'Admin'),
        (3, 'User');

        --[Users]
        INSERT INTO [dbo].[Users]
        (
            [Username],
            [Password],
            [Email],
            [CreatedDate],
            [LastLoginDate],
            [FK_User_tblRole]
        )
        VALUES
        ('super_admin', '098f6bcd4621d373cade4e832627b4f6', 'admin@test.test', GETDATE(), null, 1),
        ('admin', '098f6bcd4621d373cade4e832627b4f6', 'admin@test.test', GETDATE(), null, 2),
        ('user', '098f6bcd4621d373cade4e832627b4f6', 'admin@test.test', GETDATE(), null, 3)

		--[UserActivation]
		INSERT INTO [dbo].[UserActivation]
           (
			   [UserId],
			   [ActivationCode]
		   )
		  VALUES
           (1, newid()),
		   (2, newid()),
		   (3, newid())

        --[tblSetting]
        INSERT INTO [dbo].[tblSetting]
        (
            [title],
            [value],
            [valueType]
        )
        VALUES
        ('initialized', '1', 'bit')
    END
END
GO
/****** Object:  StoredProcedure [dbo].[spoc_logSqlError]    Script Date: 7/30/2021 12:11:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spoc_logSqlError]
    -- Add the parameters for the stored procedure here
    @ERROR_NUMBER int,
    @ERROR_SEVERITY int,
    @ERROR_STATE int,
    @ERROR_PROCEDURE nvarchar(max),
    @ERROR_LINE int,
    @ERROR_MESSAGE nvarchar(max),
    @Parameters nvarchar(max)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    --BEGIN TRY
    -- Insert statements for procedure here
    INSERT INTO [dbo].[SQL_ERROR]
    (
        [ERROR_NUMBER],
        [ERROR_SEVERITY],
        [ERROR_STATE],
        [ERROR_PROCEDURE],
        [ERROR_LINE],
        [ERROR_MESSAGE],
        [Parameters]
    )
    VALUES
    (@ERROR_NUMBER, @ERROR_SEVERITY, @ERROR_STATE, @ERROR_PROCEDURE, @ERROR_LINE, @ERROR_MESSAGE, @Parameters);
    --SELECT
    --	@ERROR_NUMBER AS ErrorNumber,
    --	@ERROR_SEVERITY AS ErrorSeverity,
    --	@ERROR_STATE AS ErrorState,
    --	@ERROR_PROCEDURE AS ErrorProcedure,
    --	@ERROR_LINE AS ErrorLine,
    --	@ERROR_MESSAGE AS ErrorMessage;
    RaisError(   @ERROR_MESSAGE,  -- Message text.  
                 @ERROR_SEVERITY, -- Severity,  
                 @ERROR_STATE     -- State,  
             );
--END TRY
--BEGIN CATCH
--SELECT
--       ERROR_NUMBER() AS ErrorNumber,
--       ERROR_SEVERITY() AS ErrorSeverity,
--       ERROR_STATE() AS ErrorState,
--       ERROR_PROCEDURE() AS ErrorProcedure,
--       ERROR_LINE() AS ErrorLine,
--       ERROR_MESSAGE() AS ErrorMessage;
--END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Validate_User]    Script Date: 7/30/2021 12:11:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Validate_User]
    @Username NVARCHAR(200),
    @Password NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @UserId INT,
            @LastLoginDate DATETIME

    SELECT @UserId = UserId,
           @LastLoginDate = LastLoginDate
    FROM Users
    WHERE Username = @Username
          AND [Password] = @Password

    IF @UserId IS NOT NULL
    BEGIN
        IF EXISTS (SELECT UserId FROM UserActivation WHERE UserId = @UserId)
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
ALTER DATABASE [PersianAdminPanel] SET  READ_WRITE 
GO
