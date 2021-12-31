SELECT a.UniqueName 'PlatformName', b.*
FROM Platform a
JOIN Well b ON (a.id = b.PlatformId)
LEFT OUTER JOIN Well c ON (a.id = c.PlatformId AND 
    (b.UpdatedAt < c.UpdatedAt OR (b.UpdatedAt = c.UpdatedAt AND b.id < c.id)))
WHERE c.id IS NULL;