-- Create User Management Tables
-- This script sets up a comprehensive user management system with roles
-- and permissions for WhiteLabels and AffiliateIDs

-- Create Roles Table
CREATE TABLE [dbo].[Roles] (
    [RoleId] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL UNIQUE,
    [Description] NVARCHAR(255) NULL,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [IsActive] BIT NOT NULL DEFAULT 1
);

-- Create Users Table
CREATE TABLE [dbo].[Users] (
    [UserId] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(50) NOT NULL UNIQUE,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [FirstName] NVARCHAR(50) NULL,
    [LastName] NVARCHAR(50) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [LastLoginDate] DATETIME NULL,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedDate] DATETIME NULL,
    [FailedLoginAttempts] INT NOT NULL DEFAULT 0,
    [LockoutEnd] DATETIME NULL
);

-- Create User-Role Associations Table
CREATE TABLE [dbo].[UserRoles] (
    [UserRoleId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    [AssignedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([RoleId]),
    CONSTRAINT [UQ_UserRoles_UserRole] UNIQUE ([UserId], [RoleId])
);

-- Create User-WhiteLabel Permissions Table
CREATE TABLE [dbo].[UserWhiteLabelPermissions] (
    [UserWhiteLabelId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [WhiteLabelId] INT NOT NULL, -- References WhiteLabels.LabelID
    [HasReadAccess] BIT NOT NULL DEFAULT 1,
    [HasWriteAccess] BIT NOT NULL DEFAULT 0,
    [AssignedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [AssignedBy] INT NULL, -- References Users.UserId
    CONSTRAINT [FK_UserWhiteLabelPermissions_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_UserWhiteLabelPermissions_AssignedBy] FOREIGN KEY ([AssignedBy]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [UQ_UserWhiteLabelPermissions] UNIQUE ([UserId], [WhiteLabelId])
);

-- Create User-AffiliateID Permissions Table
CREATE TABLE [dbo].[UserAffiliatePermissions] (
    [UserAffiliateId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [WhiteLabelId] INT NOT NULL, -- References WhiteLabels.LabelID
    [AffiliateID] NVARCHAR(100) NOT NULL,
    [HasReadAccess] BIT NOT NULL DEFAULT 1,
    [HasWriteAccess] BIT NOT NULL DEFAULT 0,
    [AssignedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [AssignedBy] INT NULL, -- References Users.UserId
    CONSTRAINT [FK_UserAffiliatePermissions_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_UserAffiliatePermissions_AssignedBy] FOREIGN KEY ([AssignedBy]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [UQ_UserAffiliatePermissions] UNIQUE ([UserId], [WhiteLabelId], [AffiliateID])
);

-- Create Login History Table
CREATE TABLE [dbo].[UserLoginHistory] (
    [LoginId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [LoginDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [IpAddress] NVARCHAR(50) NULL,
    [UserAgent] NVARCHAR(500) NULL,
    [IsSuccessful] BIT NOT NULL,
    [FailureReason] NVARCHAR(255) NULL,
    CONSTRAINT [FK_UserLoginHistory_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
);

-- Create Audit Log Table
CREATE TABLE [dbo].[AuditLog] (
    [AuditId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NULL,
    [Action] NVARCHAR(50) NOT NULL,
    [TableName] NVARCHAR(50) NOT NULL,
    [RecordId] NVARCHAR(50) NOT NULL,
    [OldValues] NVARCHAR(MAX) NULL,
    [NewValues] NVARCHAR(MAX) NULL,
    [Timestamp] DATETIME NOT NULL DEFAULT GETDATE(),
    [IpAddress] NVARCHAR(50) NULL,
    CONSTRAINT [FK_AuditLog_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
);

-- Populate Roles
INSERT INTO [dbo].[Roles] ([Name], [Description])
VALUES 
    ('Admin', 'Full system access with all privileges'),
    ('Manager', 'Can manage users and view all data'),
    ('User', 'Standard user with restricted access');

-- Create a default Admin user (password: Admin@123)
-- Note: In production, use a proper password hashing function
INSERT INTO [dbo].[Users] ([Username], [Email], [PasswordHash], [FirstName], [LastName])
VALUES ('admin', 'admin@progressplay.com', 'AQAAAAEAACcQAAAAEDxQS0Ps72bhdT5vPK/RhROj33nvG+lSX5hy2bQDc1fd2n5/c9+QgCQrL5iOOu13KQ==', 'System', 'Administrator');

-- Assign Admin role to the default admin user
INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId])
VALUES (1, 1);

-- Create indexes for better performance
CREATE INDEX [IX_Users_Username] ON [dbo].[Users] ([Username]);
CREATE INDEX [IX_Users_Email] ON [dbo].[Users] ([Email]);
CREATE INDEX [IX_UserRoles_UserId] ON [dbo].[UserRoles] ([UserId]);
CREATE INDEX [IX_UserRoles_RoleId] ON [dbo].[UserRoles] ([RoleId]);
CREATE INDEX [IX_UserWhiteLabelPermissions_UserId] ON [dbo].[UserWhiteLabelPermissions] ([UserId]);
CREATE INDEX [IX_UserWhiteLabelPermissions_WhiteLabelId] ON [dbo].[UserWhiteLabelPermissions] ([WhiteLabelId]);
CREATE INDEX [IX_UserAffiliatePermissions_UserId] ON [dbo].[UserAffiliatePermissions] ([UserId]);
CREATE INDEX [IX_UserAffiliatePermissions_WhiteLabelId] ON [dbo].[UserAffiliatePermissions] ([WhiteLabelId]);
CREATE INDEX [IX_UserAffiliatePermissions_AffiliateID] ON [dbo].[UserAffiliatePermissions] ([AffiliateID]);