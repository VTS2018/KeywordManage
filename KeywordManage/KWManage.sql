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
 

--基数据信息

--1.分类集合
create table Category_Info
(
  CID nvarchar(30) primary key not null,
  CategoryName nvarchar(20) not null,
  CategoryCode nvarchar(50) not null,
  CategoryStatus nvarchar(10),
  CateRemark nvarchar(500)
)

--2.分类选项值集合
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

--3.关键字集合
create table KeyWords
(
  KID nvarchar(30) primary key not null,
  KeyWordsName nvarchar(200) not null,
  KeyWordsStatus nvarchar(10)
)
--4.站点集合
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

--5.第三方资源集合 二级域名博客
create table WebBlog
(
  WebBlogID nvarchar(30) primary key not null,
  WebBlogURL nvarchar(200) not null,
  PR int default(0),
  LoginInfo nvarchar(500),
  WebBlogStatus nvarchar(10)
)

--6.合作伙伴集合
create table Partner
(
  PID nvarchar(30)  primary key,
  PartnerName nvarchar(20),
  PhoneNumber nvarchar(20),
  QQ nvarchar(20),
  PartnerStatus  nvarchar(10)
)

--7.注册博客用户集合
create table RegUsers
(
  UID nvarchar(30) primary key,
  UserName nvarchar(50),
  UserPassword nvarchar(100),
  UserStatus nvarchar(10)
)

/************************/
--品牌集合
create table Brand
(
 BID nvarchar(30)primary key not null,
 BrandName nvarchar(50) not null, 
 Createtime datetime,
 Remark nvarchar(500)
)



--关系信息表
--8.关键词
create table KeyWord_Brand
(
  KBID nvarchar(30) primary key not null, --主键ID
  KeyWordsID nvarchar(30),                --关键词ID
  BID nvarchar(30),                       --品牌ID
                                          --品牌子类ID
  CVID nvarchar(30),                      --国家ID
                                          --关键词分类ID ?
    
)

--9.排名站点-国家关系
create table WebSite_Country
(
  WCID nvarchar(30) primary key not null,
  WebSiteID nvarchar(30) not null,--站点ID
  WebSiteType nvarchar(30), --站点类型
  Country nvarchar(30)      --站点国家ID
                            --域名状态ID？
)

--10.第三方资源
create table WebBlogType
(
  WBTID nvarchar(30) not null,--类型ID
  BlogID int not null,  --博客ID
  BlogType nvarchar(20) --博客类型
)
--合作伙伴的关系
--11.合作伙伴和关键字的关系
create table Partner_Keyword
(
 PID nvarchar(30),
 KID nvarchar(30)
)

--12.合作伙伴和站点的关系
create table Partner_WebSite
(
 PID nvarchar(30),
 WebSiteID nvarchar(30)
)
--13.合作伙伴 站点 关键字之间的关系
create table Partner_Key_WebSite
(
 PID nvarchar(30),
 WebSiteID nvarchar(30),
 KID nvarchar(30)
)
