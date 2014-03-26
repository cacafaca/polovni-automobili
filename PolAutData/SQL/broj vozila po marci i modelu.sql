-- broj vozila po marci i modelu
select a.marka, a.model as model, count(*) broj, min(a.godinaproizvodnje) min_godiste, avg(a.godinaproizvodnje) as prosecno_godiste,
 max(a.godinaproizvodnje) max_godiste
from automobil a
group by a.marka, a.model
order by broj desc
