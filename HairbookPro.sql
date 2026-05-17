CREATE DATABASE HairBookPro;
GO

USE HairBookPro;
GO

CREATE TABLE Uloga (
    UlogaID INT IDENTITY(1,1) PRIMARY KEY,
    Naziv NVARCHAR(50) NOT NULL
);

CREATE TABLE Korisnik (
    KorisnikID INT IDENTITY(1,1) PRIMARY KEY,
    Ime NVARCHAR(50) NOT NULL,
    Prezime NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Lozinka NVARCHAR(100) NOT NULL,
    Telefon NVARCHAR(30),
    UlogaID INT NOT NULL,
    FOREIGN KEY (UlogaID) REFERENCES Uloga(UlogaID)
);

CREATE TABLE Klijent (
    KlijentID INT IDENTITY(1,1) PRIMARY KEY,
    KorisnikID INT NOT NULL UNIQUE,
    DatumRegistracije DATE NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (KorisnikID) REFERENCES Korisnik(KorisnikID)
);

CREATE TABLE Zaposleni (
    ZaposleniID INT IDENTITY(1,1) PRIMARY KEY,
    KorisnikID INT NOT NULL UNIQUE,
    DatumZaposlenja DATE NOT NULL,
    Aktivan BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (KorisnikID) REFERENCES Korisnik(KorisnikID)
);

CREATE TABLE Usluga (
    UslugaID INT IDENTITY(1,1) PRIMARY KEY,
    Naziv NVARCHAR(100) NOT NULL,
    Opis NVARCHAR(300),
    Cijena DECIMAL(10,2) NOT NULL,
    TrajanjeMinuta INT NOT NULL,
    Aktivna BIT NOT NULL DEFAULT 1
);

CREATE TABLE StatusTermina (
    StatusID INT IDENTITY(1,1) PRIMARY KEY,
    Naziv NVARCHAR(50) NOT NULL
);

CREATE TABLE Termin (
    TerminID INT IDENTITY(1,1) PRIMARY KEY,
    KlijentID INT NOT NULL,
    ZaposleniID INT NOT NULL,
    DatumVrijeme DATETIME NOT NULL,
    StatusID INT NOT NULL,
    Napomena NVARCHAR(300),
    FOREIGN KEY (KlijentID) REFERENCES Klijent(KlijentID),
    FOREIGN KEY (ZaposleniID) REFERENCES Zaposleni(ZaposleniID),
    FOREIGN KEY (StatusID) REFERENCES StatusTermina(StatusID)
);

CREATE TABLE TerminUsluga (
    TerminUslugaID INT IDENTITY(1,1) PRIMARY KEY,
    TerminID INT NOT NULL,
    UslugaID INT NOT NULL,
    FOREIGN KEY (TerminID) REFERENCES Termin(TerminID),
    FOREIGN KEY (UslugaID) REFERENCES Usluga(UslugaID)
);

CREATE TABLE NacinPlacanja (
    NacinPlacanjaID INT IDENTITY(1,1) PRIMARY KEY,
    Naziv NVARCHAR(50) NOT NULL
);

CREATE TABLE Placanje (
    PlacanjeID INT IDENTITY(1,1) PRIMARY KEY,
    TerminID INT NOT NULL UNIQUE,
    Iznos DECIMAL(10,2) NOT NULL,
    DatumPlacanja DATETIME NOT NULL DEFAULT GETDATE(),
    NacinPlacanjaID INT NOT NULL,
    FOREIGN KEY (TerminID) REFERENCES Termin(TerminID),
    FOREIGN KEY (NacinPlacanjaID) REFERENCES NacinPlacanja(NacinPlacanjaID)
);

CREATE TABLE Recenzija (
    RecenzijaID INT IDENTITY(1,1) PRIMARY KEY,
    KlijentID INT NOT NULL,
    ZaposleniID INT NOT NULL,
    Ocjena INT NOT NULL CHECK (Ocjena BETWEEN 1 AND 5),
    Komentar NVARCHAR(500),
    Datum DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (KlijentID) REFERENCES Klijent(KlijentID),
    FOREIGN KEY (ZaposleniID) REFERENCES Zaposleni(ZaposleniID)
);

CREATE TABLE RadnoVrijeme (
    RadnoVrijemeID INT IDENTITY(1,1) PRIMARY KEY,
    ZaposleniID INT NOT NULL,
    DanUSedmici NVARCHAR(20) NOT NULL,
    VrijemeOd TIME NOT NULL,
    VrijemeDo TIME NOT NULL,
    FOREIGN KEY (ZaposleniID) REFERENCES Zaposleni(ZaposleniID)
);
GO

--Test podaci
INSERT INTO Uloga (Naziv)
VALUES 
('Admin'),
('Frizer'),
('Klijent');

INSERT INTO StatusTermina (Naziv)
VALUES
('Zakazan'),
('Otkazan'),
('Zavrsen');

INSERT INTO NacinPlacanja (Naziv)
VALUES
('Gotovina'),
('Kartica');

INSERT INTO Korisnik (Ime, Prezime, Email, Lozinka, Telefon, UlogaID)
VALUES
('Ivana', 'Admin', 'admin@hairbook.com', 'admin123', '061111111', 1),
('Maja', 'Frizer', 'maja@hairbook.com', 'maja123', '062222222', 2),
('Ana', 'Klijent', 'ana@gmail.com', 'ana123', '063333333', 3);

INSERT INTO Zaposleni (KorisnikID, DatumZaposlenja)
VALUES
(2, '2025-01-10');

INSERT INTO Klijent (KorisnikID)
VALUES
(3);

INSERT INTO Usluga (Naziv, Opis, Cijena, TrajanjeMinuta)
VALUES
('Sisanje', 'Zensko sisanje', 20, 30),
('Feniranje', 'Feniranje kose', 15, 20),
('Farbanje', 'Farbanje kose', 50, 90);

