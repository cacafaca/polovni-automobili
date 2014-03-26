-- izmenjeni zapisi poređani po vremnu izmene unazad
select naslov, cena, datumizmene, vremeizmene, a.datumpostavljanja
from automobil a
order by datumizmene desc , vremeizmene desc;
