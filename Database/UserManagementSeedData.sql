-- User Management Seed Data

-- Ensure proper transaction handling
BEGIN TRANSACTION;

BEGIN TRY

    -- Insert Roles if they don't exist already
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [Name] = 'Admin')
    BEGIN
        INSERT INTO [dbo].[Roles] ([Name], [Description], [CreatedDate])
        VALUES ('Admin', 'Full system access with all privileges', GETDATE());
    END

    IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [Name] = 'Manager')
    BEGIN
        INSERT INTO [dbo].[Roles] ([Name], [Description], [CreatedDate])
        VALUES ('Manager', 'Can manage most resources and view all data', GETDATE());
    END

    IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [Name] = 'User')
    BEGIN
        INSERT INTO [dbo].[Roles] ([Name], [Description], [CreatedDate])
        VALUES ('User', 'Standard user with restricted access', GETDATE());
    END

    -- Insert Admin user if it doesn't exist
    -- Password is 'Admin@123456' (hashed)
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'admin')
    BEGIN
        INSERT INTO [dbo].[Users] (
            [Username], 
            [Email], 
            [PasswordHash], 
            [FirstName], 
            [LastName], 
            [IsActive], 
            [CreatedDate]
        )
        VALUES (
            'admin', 
            'admin@progressplay.com', 
            'AQAAAAIAAYagAAAAEKGIieH3g4qzx9nbEyfbL5xrJEO7Pca4TGxn829fSiZj1QPNM6mF4/+rSm+1RDVW8w==', -- Admin@123456
            'System', 
            'Administrator', 
            1, 
            GETDATE()
        );
    END

    -- Give Admin role to the admin user
    DECLARE @AdminRoleId INT = (SELECT [RoleId] FROM [dbo].[Roles] WHERE [Name] = 'Admin');
    DECLARE @AdminUserId INT = (SELECT [UserId] FROM [dbo].[Users] WHERE [Username] = 'admin');

    IF NOT EXISTS (SELECT 1 FROM [dbo].[UserRoles] WHERE [UserId] = @AdminUserId AND [RoleId] = @AdminRoleId)
    BEGIN
        INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId], [AssignedDate])
        VALUES (@AdminUserId, @AdminRoleId, GETDATE());
    END

    -- Admin users don't need explicit WhiteLabel permissions as they have full access by role definition
    -- The PermissionFilteredController will handle this logic by checking the Admin role

    -- Insert a Manager user
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'manager')
    BEGIN
        INSERT INTO [dbo].[Users] (
            [Username], 
            [Email], 
            [PasswordHash], 
            [FirstName], 
            [LastName], 
            [IsActive], 
            [CreatedDate]
        )
        VALUES (
            'manager', 
            'manager@progressplay.com', 
            'AQAAAAIAAYagAAAAEF8Ij5efoRJDnXueN1jgH6plIDSLbnxT63f4BMJJeQylP+JL+jG8Z82UbsZ+hAhCDw==', -- Manager@123456
            'Manager', 
            'User', 
            1, 
            GETDATE()
        );
    END

    -- Give Manager role to the manager user
    DECLARE @ManagerRoleId INT = (SELECT [RoleId] FROM [dbo].[Roles] WHERE [Name] = 'Manager');
    DECLARE @ManagerUserId INT = (SELECT [UserId] FROM [dbo].[Users] WHERE [Username] = 'manager');

    IF NOT EXISTS (SELECT 1 FROM [dbo].[UserRoles] WHERE [UserId] = @ManagerUserId AND [RoleId] = @ManagerRoleId)
    BEGIN
        INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId], [AssignedDate])
        VALUES (@ManagerUserId, @ManagerRoleId, GETDATE());
    END

    -- Grant manager access to specific WhiteLabels (276 and 277)
    IF NOT EXISTS (
        SELECT 1 
        FROM [dbo].[UserWhiteLabelPermissions] 
        WHERE [UserId] = @ManagerUserId AND [WhiteLabelId] = 276
    )
    BEGIN
        INSERT INTO [dbo].[UserWhiteLabelPermissions] 
            ([UserId], [WhiteLabelId], [HasReadAccess], [HasWriteAccess], [AssignedDate])
        VALUES 
            (@ManagerUserId, 276, 1, 0, GETDATE()),
            (@ManagerUserId, 277, 1, 0, GETDATE());
    END

    -- Insert a regular User
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'user')
    BEGIN
        INSERT INTO [dbo].[Users] (
            [Username], 
            [Email], 
            [PasswordHash], 
            [FirstName], 
            [LastName], 
            [IsActive], 
            [CreatedDate]
        )
        VALUES (
            'user', 
            'user@progressplay.com', 
            'AQAAAAIAAYagAAAAEG5rfFgvLF67xnQVl+XnSPUNXUzvVtAzCaWN5CEjJjuFUL9B8VH3YV3mVUCNTPUgnA==', -- User@123456
            'Regular', 
            'User', 
            1, 
            GETDATE()
        );
    END

    -- Give User role to the regular user
    DECLARE @UserRoleId INT = (SELECT [RoleId] FROM [dbo].[Roles] WHERE [Name] = 'User');
    DECLARE @RegularUserId INT = (SELECT [UserId] FROM [dbo].[Users] WHERE [Username] = 'user');

    IF NOT EXISTS (SELECT 1 FROM [dbo].[UserRoles] WHERE [UserId] = @RegularUserId AND [RoleId] = @UserRoleId)
    BEGIN
        INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId], [AssignedDate])
        VALUES (@RegularUserId, @UserRoleId, GETDATE());
    END

    -- Grant regular user access to only one WhiteLabel (276) with specific affiliate restrictions
    IF NOT EXISTS (
        SELECT 1 
        FROM [dbo].[UserWhiteLabelPermissions] 
        WHERE [UserId] = @RegularUserId AND [WhiteLabelId] = 276
    )
    BEGIN
        INSERT INTO [dbo].[UserWhiteLabelPermissions] 
            ([UserId], [WhiteLabelId], [HasReadAccess], [HasWriteAccess], [AssignedDate])
        VALUES 
            (@RegularUserId, 276, 1, 0, GETDATE());
    END

    -- Add specific affiliate permissions for regular user
    IF NOT EXISTS (
        SELECT 1 
        FROM [dbo].[UserAffiliatePermissions] 
        WHERE [UserId] = @RegularUserId AND [WhiteLabelId] = 276
    )
    BEGIN
        INSERT INTO [dbo].[UserAffiliatePermissions]
            ([UserId], [WhiteLabelId], [AffiliateID], [HasReadAccess], [HasWriteAccess], [AssignedDate])
        VALUES
            (@RegularUserId, 276, 'AFF001', 1, 0, GETDATE()),
            (@RegularUserId, 276, 'AFF002', 1, 0, GETDATE());
    END

    -- Commit the transaction if everything succeeded
    COMMIT TRANSACTION;
    PRINT 'User management seed data has been successfully inserted.';

END TRY
BEGIN CATCH
    -- Roll back the transaction if there was an error
    ROLLBACK TRANSACTION;
    
    -- Output error information
    PRINT 'An error occurred during seed data insertion:';
    PRINT ERROR_MESSAGE();
END CATCH;