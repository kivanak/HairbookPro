/*
=========================================================
HAIRBOOKPRO SQL OBJEKTI
=========================================================

Ova skripta dodaje obavezne napredne SQL objekte za projekat:

1. TRIGGERI
   - Automatski se izvršavaju kada se desi određena promjena u tabeli.
   - Koriste se za logovanje i arhiviranje podataka.

2. STORED PROCEDURE
   - Čuvaju SQL logiku u bazi.
   - Koriste se za detaljnu pretragu usluga i kreiranje termina.

3. FUNKCIJE
   - Vraćaju izračunate vrijednosti.
   - Koriste se za prosječnu ocjenu frizera i broj termina korisnika.
=========================================================
*/


---------------------------------------------------------
-- TRIGGER 1: Logovanje novog termina
---------------------------------------------------------
/*
Kada korisnik zakaže novi termin, u tabelu Appointments se dodaje novi red.

Ovaj trigger se automatski aktivira nakon INSERT operacije nad tabelom Appointments
i upisuje zapis u tabelu AppointmentLogs.

Svrha:
- evidentira da je termin kreiran
- omogućava adminu da vidi istoriju kreiranja termina
- pokazuje upotrebu triggera za automatsko logovanje promjena
*/
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
/*
Kada se komentar obriše iz tabele Comments, podaci o tom komentaru bi se inače izgubili.

Ovaj trigger se automatski aktivira nakon DELETE operacije nad tabelom Comments
i prije trajnog uklanjanja čuva kopiju obrisanog komentara u tabeli DeletedCommentLogs.

Svrha:
- čuva istoriju obrisanih komentara
- omogućava audit trag
- pokazuje upotrebu triggera za arhiviranje podataka
*/
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
/*
Ova procedura realizuje detaljnu pretragu usluga u salonu.

Prima više opcionalnih parametara:
- tekst za pretragu
- kategoriju
- minimalnu cijenu
- maksimalnu cijenu
- maksimalno trajanje usluge

Ako je neki parametar NULL, taj filter se ignoriše.

Svrha:
- logiku detaljne pretrage premješta iz aplikacije u bazu
- omogućava fleksibilno filtriranje
- direktno ispunjava uslov projekta za stored procedure
*/
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
/*
Ova procedura se koristi za zakazivanje termina.

Prije upisa termina provjerava da li je izabrani frizer već zauzet
u istom terminu. Ako postoji aktivan termin za tog frizera i datum,
procedura prekida izvršavanje i vraća grešku.

Svrha:
- centralizuje poslovno pravilo u bazi
- sprječava duplo zakazivanje istog frizera u istom terminu
- koristi se iz AppointmentsController-a prilikom kreiranja termina
*/
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
/*
Ova funkcija računa prosječnu ocjenu za određenog frizera.

Prima StylistId i računa prosjek svih ocjena iz tabele Reviews.
Ako frizer još nema nijednu ocjenu, funkcija vraća 0.

Svrha:
- koristi se za prikaz prosječne ocjene frizera u interfejsu
- povezuje tabelu Stylists sa tabelom Reviews
- pokazuje upotrebu SQL funkcije za izračunavanje vrijednosti
*/
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
/*
Ova funkcija računa koliko termina ima određeni korisnik.

Prima UserId i broji sve termine iz tabele Appointments koji pripadaju tom korisniku.
Ako korisnik nema termine, funkcija vraća 0.

Svrha:
- može se koristiti na stranici "Moji termini" ili admin dashboardu
- prikazuje korisničku aktivnost
- pokazuje upotrebu SQL funkcije za agregaciju podataka
*/
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