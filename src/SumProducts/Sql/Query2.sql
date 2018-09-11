SELECT SUM(Total) as Total, MONTHNAME(LastUpdate) as Month, YEAR(LastUpdate) as Year
FROM Orders
GROUP BY Month, Year