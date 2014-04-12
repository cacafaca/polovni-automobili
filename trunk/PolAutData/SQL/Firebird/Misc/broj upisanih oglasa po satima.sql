-- broj upisanih oglasa po satima
select datumizmene, extract(hour from vremeizmene) as Sat , count(*) Broj
from automobil
group by datumizmene, extract(hour from vremeizmene)
order by 1 desc ,2 desc,3 desc
