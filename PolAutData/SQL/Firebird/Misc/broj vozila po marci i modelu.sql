-- broj vozila po marci i modelu
select A.MARKA, A.MODEL as MODEL, count(*) BROJ, min(A.GODINAPROIZVODNJE) MIN_GODISTE,
       avg(A.GODINAPROIZVODNJE) as PROSECNO_GODISTE, max(A.GODINAPROIZVODNJE) MAX_GODISTE
from AUTOMOBIL A
group by A.MARKA, A.MODEL
order by BROJ desc  
