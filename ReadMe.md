-# TomoViz
Geo visualization using the Unity3D API
Current version is created in Unity 2018.1

Directions to export data sets from MatLab to Unity

/* This on down is the required Algorithm
/* For a loaded srModel specifically in our cascadia project



fid = fopen('x.dat', 'a+');

lat = srElevation.LAT
Q = lat(:)
b=size(lat,1) * size(lat,2)

fprintf(fid, '%d %d\n', [size(lat, 1), size(lat, 2)]);

for b=1:size(Q)
  fprintf(fid, '%d\n', Q(b));
end


lon =srElevation.LON
Q = lon(:)
b=size(lon,1) * size(lon,2)

fprintf(fid, '%d %d\n', [size(lon, 1), size(lon, 2)]);

for b=1:size(Q)
  fprintf(fid, '%d\n', Q(b));
end


elev = srElevation.data;

Q = elev(:)
b=size(elev,1) * size(elev,2)

fprintf(fid, '%d %d\n', [size(elev, 1), size(elev, 2)]);

for b=1:size(Q)
  fprintf(fid, '%d\n', Q(b));
end


lat = srModel.LAT
Q = lat(:)
b=size(lat,1) * size(lat,2)

fprintf(fid, '%d %d\n', [size(lat, 1), size(lat, 2)]);

for b=1:size(Q)
  fprintf(fid, '%d\n', Q(b));
end


lon = srModel.LON
Q = lon(:)
b=size(lon,1) * size(lon,2)

fprintf(fid, '%d %d\n', [size(lon, 1), size(lon, 2)]);

for b=1:size(Q)
  fprintf(fid, '%d\n', Q(b));
end


volDepth = srModel.zg

Q = volDepth(:)
b=size(volDepth,1) * size(volDepth,2)

fprintf(fid, '%d %d\n', [size(volDepth, 1), size(volDepth, 2)]);

for b=1:size(Q)
  fprintf(fid, '%d\n', Q(b));
end

data = srModel.vel_pert;
Q = data(:)
b=size(data,1) *size(data,2) *size(data,3)


fprintf(fid, '%d %d %d\n', [size(data, 1), size(data, 2), size(data, 3)]);

for b=1:size(Q)
  fprintf(fid, '%d\n', Q(b));
end


fclose(fid);