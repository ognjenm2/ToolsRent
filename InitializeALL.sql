 
CREATE TABLE Tools (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ToolKind NVARCHAR(100),
    Price DECIMAL(10,2)
);

 
DECLARE @ToolNames TABLE (ToolName NVARCHAR(100));

INSERT INTO @ToolNames (ToolName)
VALUES
    ('Čekič'), ('Šarafciger'), ('Ključ'), ('Bušilica'), ('Kliješta'), ('Pila'),
    ('Dlijeto'), ('Libela'), ('Metar'), ('Ravnalo'), ('Olovka'), ('Brusilica'),
    ('Preša'), ('Bacač plamena'), ('Bomba'), ('Dinamit'), ('Pajser'), ('Glue Gun'), ('Pero od čudnovatog kljunaša'),
    ('Kopač tunela'), ('Motorna pila'), ('Ooooogromni čekič');

 
DECLARE @ToolName NVARCHAR(100);
DECLARE @RandomSuffix NVARCHAR(10);
DECLARE @RandomPrice DECIMAL(10,2);

DECLARE @Counter INT = 1;
WHILE @Counter <= 50000
BEGIN
    SELECT TOP 1 @ToolName = ToolName FROM @ToolNames ORDER BY NEWID();
    SET @RandomSuffix = CAST(ABS(CHECKSUM(NEWID())) % 10000 AS NVARCHAR(10));  
    SET @RandomPrice = ROUND(10 + (2990 * RAND()), 2); 

    INSERT INTO Tools (ToolKind, Price)
    VALUES (CONCAT(@ToolName, '-', @RandomSuffix), @RandomPrice);

    SET @Counter = @Counter + 1;
END;

CREATE TABLE Reservations (
    ReservationID INT IDENTITY(1,1) PRIMARY KEY,
    ImePrezime NVARCHAR(255),
    OfferDateTime DATETIME,
    Note NVARCHAR(MAX)
);

CREATE TABLE ToolsReservations (
    ToolReservationID INT IDENTITY(1,1) PRIMARY KEY,
    ReservationID INT FOREIGN KEY REFERENCES [dbo].[Reservations](ReservationID),
    ToolID INT FOREIGN KEY REFERENCES [dbo].[Tools](ID),
    DateFrom DATETIME,
    DateTo DATETIME,
    Price DECIMAL(10, 2) 
);

 
DECLARE @ReservationCounter INT = 1;

 
DECLARE @Names TABLE (Name NVARCHAR(100));
DECLARE @Surnames TABLE (Surname NVARCHAR(100));

INSERT INTO @Names (Name)
VALUES
    ('Ana'), ('Ivan'), ('Marko'), ('Petar'), ('Jelena'), ('Marija'), ('Nikola'), ('Štef'), ('Maja'), ('Marina'),
    ('Luka'), ('Marijana'), ('Joze'), ('Đorđ'), ('Sara'), ('Bojan'), ('MIRKO'), ('FILIP'), ('Darko'), ('Tamara');

INSERT INTO @Surnames (Surname)
VALUES
  ('Horvat'), ('Kovač'), ('Babić'), ('Marić'), ('Novak'), ('Horvat'), ('Kovačević'), ('Jurić'), ('Knežević'), ('Vuković'),
    ('Kovačić'), ('Matić'), ('Pavlović'), ('Marković'), ('Petrović'), ('Đurić'), ('Jakovljević'), ('Blagojević'), ('Nikolić'), ('Grgić');
 
WHILE @ReservationCounter <= 10000
BEGIN
 
    DECLARE @RandomName NVARCHAR(100);
    DECLARE @RandomSurname NVARCHAR(100);

    SELECT TOP 1 @RandomName = Name FROM @Names ORDER BY NEWID();
    SELECT TOP 1 @RandomSurname = Surname FROM @Surnames ORDER BY NEWID();

 
    DECLARE @OfferDateTime DATETIME = GETDATE();
    INSERT INTO [dbo].[Reservations](ImePrezime, OfferDateTime, Note)
    VALUES (CONCAT(@RandomName, ' ', @RandomSurname), @OfferDateTime, 'Note for Reservation ' + CAST(@ReservationCounter AS NVARCHAR(10)));

 
    DECLARE @ToolCounter INT = 1;
    DECLARE @NumTools INT = ROUND(RAND() * 4, 0) + 1; 
    
    WHILE @ToolCounter <= @NumTools
    BEGIN
 
        DECLARE @RandomToolID INT;
        DECLARE @ToolPrice DECIMAL(10, 2);
        SELECT TOP 1 @RandomToolID = ID, @ToolPrice = Price FROM Tools ORDER BY NEWID();
        
    
        DECLARE @DateFrom DATETIME = DATEADD(DAY, ROUND(RAND() * 30,0), @OfferDateTime);  
        DECLARE @DateTo DATETIME = DATEADD(DAY, ROUND(RAND() * 7,0) + 1, @DateFrom); 
        DECLARE @DaysDifference INT = DATEDIFF(DAY, @OfferDateTime, @DateFrom);
        DECLARE @AdjustedPrice DECIMAL(10, 2) = CASE 
                                                    WHEN @DaysDifference <= 7 THEN @ToolPrice * 0.8 
                                                    WHEN @DaysDifference <= 14 THEN @ToolPrice * 0.9 
                                                    ELSE @ToolPrice  
                                                END;

        
        INSERT INTO ToolsReservations (ReservationID, ToolID, DateFrom, DateTo, Price)
        VALUES (@ReservationCounter, @RandomToolID, @DateFrom, @DateTo, @AdjustedPrice);
        
        SET @ToolCounter = @ToolCounter + 1;
    END;

    SET @ReservationCounter = @ReservationCounter + 1;
END;
