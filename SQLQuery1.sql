truncate table PasswordGenerator;

select count (*) from PasswordGenerator;

insert into PasswordGenerator values('ABCDEfghij12345!@#$%');

update PasswordGenerator set Password='12345)(*&^ABCVFijklo' where id=2;

select * from PasswordGenerator order by Id;