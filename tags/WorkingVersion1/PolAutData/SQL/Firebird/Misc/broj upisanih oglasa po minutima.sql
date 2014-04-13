-- broj upisanih oglasa po minutima
select datumizmene, extract(hour from vremeizmene) as Sat , extract(minute from vremeizmene) as Minut, count(*) Broj
from automobil
group by datumizmene, extract(hour from vremeizmene), extract(minute from vremeizmene)
order by 1 desc ,2 desc,3 desc
