SELECT
YEAR(LastUpdate) AS Year,
SUM(IF(MONTH(LastUpdate)=1,Total,NULL)) AS A1,
SUM(IF(MONTH(LastUpdate)=2,Total,NULL)) AS A2,
SUM(IF(MONTH(LastUpdate)=3,Total,NULL)) AS A3,
SUM(IF(MONTH(LastUpdate)=4,Total,NULL)) AS A4,
SUM(IF(MONTH(LastUpdate)=5,Total,NULL)) AS A5,
SUM(IF(MONTH(LastUpdate)=6,Total,NULL)) AS A6,
SUM(IF(MONTH(LastUpdate)=7,Total,NULL)) AS A7,
SUM(IF(MONTH(LastUpdate)=8,Total,NULL)) AS A8,
SUM(IF(MONTH(LastUpdate)=9,Total,NULL)) AS A9,
SUM(IF(MONTH(LastUpdate)=10,Total,NULL)) AS A10,
SUM(IF(MONTH(LastUpdate)=11,Total,NULL)) AS A11,
SUM(IF(MONTH(LastUpdate)=12,Total,NULL)) AS A12
FROM Orders
GROUP BY Year