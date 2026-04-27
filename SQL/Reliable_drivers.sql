Select driver_id,
       COUNT(*) AS total_deliveries,
       Round(SUM(CASE WHEN delivery_status = 'COMPLETED' THEN 1 ELSE 0 END) * 100.0 / COUNT(*), 2) AS success_rate
       FROM deliveries
where attempt_timestamp >= '2024-09-01' AND attempt_timestamp < '2024-10-01'
group by driver_id
having 
sum(CASE WHEN delivery_status = 'COMPLETED' THEN 1 ElSE 0 END) >= 5
AND (SUM(CASE WHEN delivery_status = 'COMPLETED' THEN 1 ELSE 0 END) * 100.0 / COUNT(*)) >= 0.9
ORDER BY success_rate DESC; 