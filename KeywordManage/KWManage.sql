use master 
go
if exists(select * from sysdatabases where name='KWManage')
  begin 
    drop database KWManage
 end
go 
create database KWManage
go 
use KWManage
go

--declare @a int 
--set @a = 1 
--while @a<100000 
--begin 
--INSERT INTO dbo.KeyWords VALUES ('bw'+CAST(@a AS nvarchar(10)),'abc KWManage KWManage','yes') 
--set @a = @a + 1 
--end 
 

--��������Ϣ

--1.���༯��
create table Category_Info
(
  CID nvarchar(30) primary key not null,
  CategoryName nvarchar(20) not null,
  CategoryCode nvarchar(50) not null,
  CategoryStatus nvarchar(10),
  CateRemark nvarchar(500)
)

--2.����ѡ��ֵ����
create table Category_Value
(
  CVID nvarchar(30) primary key not null,
  CategoryCode nvarchar(50) not null,
  CateText nvarchar(20) not null,
  CateValue nvarchar(20) not null,
  CateSeleteStatue nvarchar(20),
  CateActivity nvarchar(10),
  CateSortIndex int ,
)

--3.�ؼ��ּ���
create table KeyWords
(
  KID nvarchar(30) primary key not null,
  KeyWordsName nvarchar(200) not null,
  KeyWordsStatus nvarchar(10)
)
--4.վ�㼯��
create table WebSite
(
  WebSiteID nvarchar(30)  primary key not null,
  WebSiteName nvarchar(50) not null,
  Remark nvarchar(250),
  LoginInfo nvarchar(500),
  Onlinetime datetime,
  GoogleIndextime datetime,
  Killtimetime datetime,
  LogoutTime datetime,
  WebSiteStatue nvarchar(10)
)

--5.��������Դ���� ������������
create table WebBlog
(
  WebBlogID nvarchar(30) primary key not null,
  WebBlogURL nvarchar(200) not null,
  PR int default(0),
  LoginInfo nvarchar(500),
  WebBlogStatus nvarchar(10)
)

--6.������鼯��
create table Partner
(
  PID nvarchar(30)  primary key,
  PartnerName nvarchar(20),
  PhoneNumber nvarchar(20),
  QQ nvarchar(20),
  PartnerStatus  nvarchar(10)
)

--7.ע�Ჩ���û�����
create table RegUsers
(
  UID nvarchar(30) primary key,
  UserName nvarchar(50),
  UserPassword nvarchar(100),
  UserStatus nvarchar(10)
)

/************************/
--Ʒ�Ƽ���
create table Brand
(
 BID nvarchar(30)primary key not null,
 BrandName nvarchar(50) not null, 
 Createtime datetime,
 Remark nvarchar(500)
)



--��ϵ��Ϣ��
--8.�ؼ���
create table KeyWord_Brand
(
  KBID nvarchar(30) primary key not null, --����ID
  KeyWordsID nvarchar(30),                --�ؼ���ID
  BID nvarchar(30),                       --Ʒ��ID
                                          --Ʒ������ID
  CVID nvarchar(30),                      --����ID
                                          --�ؼ��ʷ���ID ?
    
)

--9.����վ��-���ҹ�ϵ
create table WebSite_Country
(
  WCID nvarchar(30) primary key not null,
  WebSiteID nvarchar(30) not null,--վ��ID
  WebSiteType nvarchar(30), --վ������
  Country nvarchar(30)      --վ�����ID
                            --����״̬ID��
)

--10.��������Դ
create table WebBlogType
(
  WBTID nvarchar(30) not null,--����ID
  BlogID int not null,  --����ID
  BlogType nvarchar(20) --��������
)
--�������Ĺ�ϵ
--11.�������͹ؼ��ֵĹ�ϵ
create table Partner_Keyword
(
 PID nvarchar(30),
 KID nvarchar(30)
)

--12.��������վ��Ĺ�ϵ
create table Partner_WebSite
(
 PID nvarchar(30),
 WebSiteID nvarchar(30)
)
--13.������� վ�� �ؼ���֮��Ĺ�ϵ
create table Partner_Key_WebSite
(
 PID nvarchar(30),
 WebSiteID nvarchar(30),
 KID nvarchar(30)
)
