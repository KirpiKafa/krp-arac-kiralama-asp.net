CREATE TABLE [dbo].[Tbl_Rezervasyonlar] (
    [RezervasyonId] INT           IDENTITY (1, 1) NOT NULL,
    [AracId]        INT           NULL,
    [TcKimlik]      VARCHAR (11)  NULL,
    [AdSoyad]       NVARCHAR (50) NULL,
    [AlmaTarihi]    DATE          NULL,
    [TeslimTarihi]  DATE          NULL,
    [Ucret]         MONEY         NULL,
    [IptalMi]       VARCHAR (10)  NULL,
    [IptalTarihi]   DATE          NULL,
    PRIMARY KEY CLUSTERED ([RezervasyonId] ASC)
);


GO
create trigger IptalTarihi_Olustur
on Tbl_Rezervasyonlar
after update
as
begin
declare @iptal_txt varchar(10)
declare @rezId int
select @rezId=RezervasyonId, @iptal_txt=IptalMi from inserted
if @iptal_txt='Evet'
begin
 update Tbl_Rezervasyonlar set IptalTarihi=getdate() where
RezervasyonId=@rezId
end
end