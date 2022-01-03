create database SimpleChat
use SimpleChat

set dateformat DMY

--User
go
create table UserBase (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	FirstName nvarchar(100),
	LastName nvarchar(100),
	Town nvarchar(100),
	Email nvarchar(100),
	Password nvarchar(100),
	DoB datetime,
	Gender int,
)

--FileMap
go
create table FileMap (
	Id UNIQUEIDENTIFIER,
	FileName nvarchar,
	PRIMARY KEY (Id)
)

--Message
go
create table MessageBase (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	AuthorId UNIQUEIDENTIFIER,
	Revoked int,
	MessageType int,
	FOREIGN KEY (AuthorId) REFERENCES UserBase(Id)
)
go
create table MessageInteract (
	Id UNIQUEIDENTIFIER,
	UserId UNIQUEIDENTIFIER,
	Seen int,
	Hide int,
	React int,
	PRIMARY KEY (Id, UserId),
	FOREIGN KEY (UserId) REFERENCES UserBase(Id),
	FOREIGN KEY (Id) REFERENCES MessageBase(Id)
)

--1. AnnouncementMessage
--2. TextMessage
go
create table TextMessage (
	Id UNIQUEIDENTIFIER,
	Content nvarchar,
	PRIMARY KEY (Id),
	FOREIGN KEY (Id) REFERENCES MessageBase(Id)
)

--1. ImageMessage
--2. VideoMessage
--3. AttachmentMessage
go
create table MediaMessage (
	Id UNIQUEIDENTIFIER,
	FileId UNIQUEIDENTIFIER,
	FileName nvarchar(100),
	PRIMARY KEY (Id),
	FOREIGN KEY (Id) REFERENCES MessageBase(Id),
	FOREIGN KEY (FileId) REFERENCES FileMap(Id)
)
go
create table StickerMessage (
	Id UNIQUEIDENTIFIER,
	StickerId int,
	CategoryId int,
	PRIMARY KEY (Id),
	FOREIGN KEY (Id) REFERENCES MessageBase(Id)
)

--Notification
go
create table NotificationBase (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	TargetUserId UNIQUEIDENTIFIER,
	CreateTime bigint,
	NotificationType int,
	FOREIGN KEY (TargetUserId) REFERENCES UserBase(Id),
)
go
create table CommunicateNotification (
	Id UNIQUEIDENTIFIER,
	SenderUserId UNIQUEIDENTIFIER,
	SenderName nvarchar(100),
	PRIMARY KEY (Id),
	FOREIGN KEY (Id) REFERENCES NotificationBase(Id),
	FOREIGN KEY (SenderUserId) REFERENCES UserBase(Id),
)

--Conversation
go
create table ConversationBase (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	LastActive BIGINT,
	Color INT,
)
go
create table ConversationMember (
	Id UNIQUEIDENTIFIER,
	UserId UNIQUEIDENTIFIER,
	Nickname nvarchar(100),
	PRIMARY KEY (Id, UserId),
	FOREIGN KEY (Id) REFERENCES ConversationBase(Id),
	FOREIGN KEY (UserId) REFERENCES UserBase(Id)
)
go
create table ConversationMessage (
	Id UNIQUEIDENTIFIER,
	MessageId UNIQUEIDENTIFIER,
	PRIMARY KEY (Id, MessageId),
	FOREIGN KEY (Id) REFERENCES ConversationBase(Id),
	FOREIGN KEY (MessageId) REFERENCES MessageBase(Id)
)
go
create table ConversationMedia (
	Id UNIQUEIDENTIFIER,
	MessageId UNIQUEIDENTIFIER,
	PRIMARY KEY (Id, MessageId),
	FOREIGN KEY (Id) REFERENCES ConversationBase(Id),
	FOREIGN KEY (MessageId) REFERENCES MessageBase(Id)
)
go
create table ConversationAttachment (
	Id UNIQUEIDENTIFIER,
	MessageId UNIQUEIDENTIFIER,
	PRIMARY KEY (Id, MessageId),
	FOREIGN KEY (Id) REFERENCES ConversationBase(Id),
	FOREIGN KEY (MessageId) REFERENCES MessageBase(Id)
)
go
create table ChatUser (
	Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	LastLogon datetime,
	LastLogoff datetime,
	Banned int,
	FOREIGN KEY (Id) REFERENCES UserBase(Id)
)
go
create table ChatUserTheme (
	UserId UNIQUEIDENTIFIER,
	Background nvarchar(100),
	BackgroundBlur int,
	BackgroundColor int,
	BackgroundType int,
	IconColor int,
	PRIMARY KEY (UserId),
	FOREIGN KEY (UserId) REFERENCES ChatUser(Id)
)
go
create table ChatUserNotification (
	UserId UNIQUEIDENTIFIER,
	NotificationId UNIQUEIDENTIFIER,
	PRIMARY KEY (UserId, NotificationId),
	FOREIGN KEY (UserId) REFERENCES ChatUser(Id),
	FOREIGN KEY (NotificationId) REFERENCES NotificationBase(Id)
)
go
create table ChatUserRelation (
	UserOne UNIQUEIDENTIFIER,
	UserTwo UNIQUEIDENTIFIER,
	RelationType int,
	PRIMARY KEY (UserOne, UserTwo),
	FOREIGN KEY (UserOne) REFERENCES UserBase(Id),
	FOREIGN KEY (UserTwo) REFERENCES UserBase(Id)
)