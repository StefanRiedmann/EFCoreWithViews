select distinct 
    Gardens.GardenId
    , Gardens.Name
    , ca.Year
    ,substring(
        (
            select ' | '+Crops.Name as [text()]
            from Crops
            join CropAssignments on Crops.CropId = CropAssignments.CropId
            join Beds on Beds.BedId = CropAssignments.BedId
            where Beds.GardenId = Gardens.GardenId and ca.[Year] = CropAssignments.[Year]
            order by Crops.Name
            FOR XML PATH ('')
        ), 4, 1000) as AllGardenCrops
    from Gardens
    join Beds on Beds.GardenId = Gardens.GardenId
    join CropAssignments as ca on ca.BedId = Beds.BedId
