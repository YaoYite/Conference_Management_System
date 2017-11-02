select UserName, title from AspNetUsers u
join AspNetUserRoles r on u.Id = r.UserId
join assign a on u.Id = a.reviewer_id
join paper p on a.paper_id = p.Id;