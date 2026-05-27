/*
HairBookPro SQL objekti:
- 2 triggera
- 2 stored procedure
- 2 funkcije

Pokrenuti nakon EF migracija u SQL Server Management Studio.
Po potrebi provjeri tačna imena tabela ako EF napravi pluralizaciju drugačije.
*/

---------------------------------------------------------
-- TRIGGER 1: Logovanje novog termina
---------------------------------------------------------
CREATE OR ALTER TRIGGER trg_Appointment_Insert_Log
ON dbo.Appointments
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.AppointmentLogs (AppointmentId, Action, CreatedAt)
    SELECT Id, 'CREATED', GETDATE()
    FROM inserted;
END;
GO

---------------------------------------------------------
-- TRIGGER 2: Arhiviranje obrisanih komentara
---------------------------------------------------------
CREATE OR ALTER TRIGGER trg_Comment_Delete_Archive
ON dbo.Comments
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.DeletedCommentLogs (CommentId, BlogPostId, UserId, Content, DeletedAt)
    SELECT Id, BlogPostId, UserId, Content, GETDATE()
    FROM deleted;
END;
GO

---------------------------------------------------------
-- STORED PROCEDURE 1: Detaljna pretraga usluga
---------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_SearchServices
    @Query NVARCHAR(120) = NULL,
    @CategoryId INT = NULL,
    @MinPrice DECIMAL(18,2) = NULL,
    @MaxPrice DECIMAL(18,2) = NULL,
    @MaxDurationMinutes INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT s.*
    FROM dbo.Services s
    LEFT JOIN dbo.ServiceCategories c ON c.Id = s.ServiceCategoryId
    WHERE s.IsActive = 1
      AND (@Query IS NULL OR s.Name LIKE '%' + @Query + '%' OR s.Description LIKE '%' + @Query + '%')
      AND (@CategoryId IS NULL OR s.ServiceCategoryId = @CategoryId)
      AND (@MinPrice IS NULL OR s.Price >= @MinPrice)
      AND (@MaxPrice IS NULL OR s.Price <= @MaxPrice)
      AND (@MaxDurationMinutes IS NULL OR s.DurationMinutes <= @MaxDurationMinutes)
    ORDER BY s.Price ASC;
END;
GO

---------------------------------------------------------
-- STORED PROCEDURE 2: Kreiranje termina uz provjeru zauzetosti
---------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_CreateAppointment
    @UserId NVARCHAR(128),
    @ServiceId INT,
    @StylistId INT,
    @AppointmentDate DATETIME,
    @Note NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM dbo.Appointments
        WHERE StylistId = @StylistId
          AND AppointmentDate = @AppointmentDate
          AND Status <> 'Cancelled'
    )
    BEGIN
        RAISERROR('Izabrani termin je zauzet.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.Appointments (UserId, ServiceId, StylistId, AppointmentDate, Status, Note, CreatedAt)
    VALUES (@UserId, @ServiceId, @StylistId, @AppointmentDate, 'Pending', @Note, GETDATE());
END;
GO

---------------------------------------------------------
-- FUNCTION 1: Prosječna ocjena frizera
---------------------------------------------------------
CREATE OR ALTER FUNCTION fn_GetAverageStylistRating (@StylistId INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @Average DECIMAL(10,2);

    SELECT @Average = CAST(AVG(CAST(Rating AS DECIMAL(10,2))) AS DECIMAL(10,2))
    FROM dbo.Reviews
    WHERE StylistId = @StylistId;

    RETURN ISNULL(@Average, 0);
END;
GO

---------------------------------------------------------
-- FUNCTION 2: Broj termina korisnika
---------------------------------------------------------
CREATE OR ALTER FUNCTION fn_GetUserAppointmentCount (@UserId NVARCHAR(128))
RETURNS INT
AS
BEGIN
    DECLARE @Count INT;

    SELECT @Count = COUNT(*)
    FROM dbo.Appointments
    WHERE UserId = @UserId;

    RETURN ISNULL(@Count, 0);
END;
GO
