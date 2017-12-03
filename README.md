# TomoViz
Geo visualization using the Unity3D API
Current version is created in Unity Beta 2017.3.1f

Directions to export data sets from MatLab to Unity

/* This on down is the required Algorithm
/* For a loaded srModel specifically in our cascadia project



elev = srModel.elevation;

Q = elev(:)
b=size(elev,1) * size(elev,2)

fid = fopen('dimen.dat', 'a+');

fprintf(fid, '%d %d\n', [size(elev, 1), size(elev, 2)]);

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
