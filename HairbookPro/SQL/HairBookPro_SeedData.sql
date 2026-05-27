INSERT INTO dbo.ServiceCategories (Name) VALUES
('Šišanje'),
('Farbanje'),
('Njega kose'),
('Stilizovanje');

INSERT INTO dbo.Stylists (FullName, Specialization, Bio, IsActive) VALUES
('Ana Petrović', 'Šišanje i feniranje', 'Senior frizer sa 8 godina iskustva.', 1),
('Marko Jovanović', 'Farbanje', 'Specijalista za koloraciju i balayage.', 1),
('Milica Kovač', 'Njega kose', 'Tretmani i obnova oštećene kose.', 1);

INSERT INTO dbo.Services (Name, Description, Price, DurationMinutes, IsActive, ServiceCategoryId) VALUES
('Žensko šišanje', 'Profesionalno žensko šišanje.', 20, 45, 1, 1),
('Muško šišanje', 'Klasično i moderno muško šišanje.', 12, 30, 1, 1),
('Balayage', 'Moderna tehnika farbanja.', 80, 180, 1, 2),
('Keratin tretman', 'Obnova i zaglađivanje kose.', 60, 120, 1, 3),
('Feniranje', 'Stilizovanje i feniranje.', 15, 35, 1, 4);

INSERT INTO dbo.BlogCategories (Name) VALUES
('Savjeti'),
('Trendovi'),
('Njega');

INSERT INTO dbo.BlogPosts (Title, Content, CreatedAt, BlogCategoryId) VALUES
('Kako njegovati kosu tokom ljeta', 'Koristite hidratantne maske i zaštitu od sunca...', GETDATE(), 3),
('Top frizure ove sezone', 'Ove sezone dominiraju prirodni talasi i slojevito šišanje...', GETDATE(), 2);
