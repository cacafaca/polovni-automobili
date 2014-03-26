-- broj vozila po marci
select a.marka,  count(*) broj, min(a.godinaproizvodnje) min_godiste, avg(a.godinaproizvodnje) as prosecno_godiste,
 max(a.godinaproizvodnje) max_godiste
from automobil a
group by a.marka
order by broj desc
