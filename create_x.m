% TomoViz Geo visualization using the Unity3D API Current version is created in Unity 8.1
% 
% Directions to export data sets from MatLab to Unity
% 
% This on down is the required Algorithm /* For a loaded srModel specifically in our cascadia project

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%   set inputs
true_velocity = 0

the_model = '/Volumes/research/users/mbodmer/Cascadia/Tomography/test_tomoviz/Cascadia/srModel_vis.mat';

the_elevation = '/Volumes/research/users/mbodmer/Cascadia/Tomography/test_tomoviz/Cascadia/srElevation_vis.mat' ;

the_geometry = '/Volumes/research/users/mbodmer/Cascadia/Tomography/TomoLab_input_files/srGeometry_large.mat';

the_control    = '/Volumes/research/users/mbodmer/Cascadia/Tomography/TomoLab_input_files/srControl_test.mat';

starting_model = '/Volumes/research/users/mbodmer/Cascadia/Tomography/TomoLab_input_files/srModel_ShallowStarting_onoff_P_newvpvs.mat';

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%  load the TomoLab structures

srControl = load_srControl(the_control);

srGeometry = load_srGeometry(the_geometry);

srElevation = load_srElevation(the_elevation, srGeometry);

srModel = load_srModel(the_model, srControl, srGeometry, srElevation);

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%   remove the old file or else it will append

delete 'x.dat'

fid = fopen('x.dat', 'a+');


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%   Obtain and write latitude and longitude data of topography

lat = srElevation.LAT; Q = lat(:); b=size(lat,1) * size(lat,2)

fprintf(fid, '%d %d\n', [size(lat, 1), size(lat, 2)]);

for b=1:size(Q) fprintf(fid, '%d\n', Q(b)); end

lon = srElevation.LON; Q = lon(:); b=size(lon,1) * size(lon,2)

fprintf(fid, '%d %d\n', [size(lon, 1), size(lon, 2)]);

for b=1:size(Q) fprintf(fid, '%d\n', Q(b)); end


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%   Obtain and write topography elevation 

elev = srElevation.data;

Q = elev(:); b=size(elev,1) * size(elev,2)

fprintf(fid, '%d %d\n', [size(elev, 1), size(elev, 2)]);

for b=1:size(Q) fprintf(fid, '%d\n', Q(b)); end

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%   Obtain and write latitude and longitude data of model

lat = srModel.LAT; Q = lat(:); b=size(lat,1) * size(lat,2)

fprintf(fid, '%d %d\n', [size(lat, 1), size(lat, 2)]);

for b=1:size(Q) fprintf(fid, '%d\n', Q(b)); end

lon = srModel.LON; Q = lon(:); b=size(lon,1) * size(lon,2)

fprintf(fid, '%d %d\n', [size(lon, 1), size(lon, 2)]);

for b=1:size(Q) fprintf(fid, '%d\n', Q(b)); end


if true_velocity == 0
    %data = srModel.vel_pert; Q = data(:); b=size(data,1) *size(data,2) *size(data,3)
    
    srModel_starting = load_srModel(starting_model, srControl, srGeometry, srElevation);
    
    data = (1./srModel.u - 1./srModel_starting.u).*srModel.u*100; Q = data(:);
    
    
else
    
    data = 1./srModel.u; Q = data(:);
    
end

b=size(data,1) *size(data,2) *size(data,3)

fprintf(fid, '%d %d %d\n', [size(data, 1), size(data, 2), size(data, 3)]);

for b=1:size(Q) fprintf(fid, '%d\n', Q(b)); end

fclose(fid);