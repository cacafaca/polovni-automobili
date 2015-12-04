-- broj upisanih oglasa po minutima
select extract(day from A.DATUM_AZURIRANJA), extract(hour from A.DATUM_AZURIRANJA) as SAT,
       extract(minute from A.DATUM_AZURIRANJA) as MINUT, count(*) BROJ
from AUTOMOBIL A
group by extract(day from A.DATUM_AZURIRANJA), extract(hour from A.DATUM_AZURIRANJA), extract(minute from A.DATUM_AZURIRANJA)
order by 1 desc, 2 desc, 3 desc  
