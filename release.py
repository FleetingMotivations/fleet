from ftplib import FTP 
import datetime
import zipfile
import getpass
import ntpath
import sys
import os
from os.path import basename

def timeStamp(fmt='%Y-%m-%d-%H-%M-%S'):
	return datetime.datetime.now().strftime(fmt)

def zipdir(path, ziph):
	for root, dirs, files in os.walk(path):
		for file in files:
			ziph.write(os.path.join(root, file), basename(os.path.join(root, file)))

def connect_to_ftp():
	return_ftps = None
	i = 0
	while i <= 2:
		password = getpass.getpass();
		try:
			ftps = FTP('server134.web-hosting.com','fleet@alistair.io', password) 
			return_ftps = ftps
			break

		except Exception, e: 
			print str(e)

		i+=1

	return return_ftps;

def upload_build(path_to_build, preset_used):
	if os.path.exists(path_to_build):
		date_time = timeStamp()

		build_dir = 'pushed_builds/'
		name = 'build_'+date_time+'.zip';
		file_path = build_dir+name;

		if not os.path.exists(build_dir):
			print("creating build dir...")
			os.makedirs(build_dir)
			print("complete.")

		print("zipping...")
		zipf = zipfile.ZipFile(file_path, 'w', zipfile.ZIP_DEFLATED)

		if os.path.isfile(path_to_build):
			zipf.write(path_to_build)
		else:
			zipdir(path_to_build, zipf)
			
		zipf.close()

		print("complete.")

		ftps = connect_to_ftp()
				
		if ftps != None:
			print("uploading...")

			file = open(file_path,'rb')
			ftps.storbinary('STOR '+name, file);
			file.close()       
			ftps.quit()

			print("complete.")
		
	else:
		print("given path was incorrect")
		
		if preset_used:
			print("when using --debug or --release ensure this script is in the fleet solution folder")

def download_build(path_to_unpack, select_file):
	if path_to_unpack == '':
		print("unpacking in current dir")
		path_to_unpack = '.'

	if not os.path.exists(path_to_unpack):
		print("creating build dir...")
		os.makedirs(path_to_unpack)
		print("complete.")

	ftps = connect_to_ftp()

	if ftps != None:
		file_path = path_to_unpack+'/build.zip'
		file = open(file_path, 'wb')

		file_list = ftps.nlst('*.zip')
		file_list.sort(key=lambda x: datetime.datetime.strptime(x, 'build_%Y-%m-%d-%H-%M-%S.zip'))

		file_to_download = None

		if select_file:
			print("select file: ")

			for file_name in file_list:
				print("\t"+file_name)

			while file_to_download == None:
				var = raw_input("enter filename: ")
				if var in file_list:
					file_to_download = var
					print(file_to_download+" selected")
				else:
					print("file does not exist")

		else:
			#get the latest file based on name datestamp (or upload time)
			file_to_download = file_list[-1]
		

		print("downloading...")
		ftps.retrbinary('RETR '+file_to_download, file.write);
		file.close()
		print("complete.")

		print("unpacking...")
		if os.path.isfile(file_path):
			zip_ref = zipfile.ZipFile(file_path, 'r')
			zip_ref.extractall(path_to_unpack)
			zip_ref.close()
		print("complete.")



if len(sys.argv) <= 1:
	print("Pushing Fleet to the server so we can download releases on dev machines");
	print("Usage: python release.py pathToBuild | (--debug | -d) | (--release | -r) | (pull <pathToUnpack> <--select>) ");

else:

	upload = True
	preset_used = True

	if sys.argv[1] == '--debug' or sys.argv[1] == '-d':
		path_to_build = 'Fleet/bin/Debug/'
	elif sys.argv[1] == '--release' or sys.argv[1] == '-r':
		path_to_build = 'Fleet/bin/Release/'

	elif sys.argv[1] == 'pull':
		upload = False
		select_file = False
		if len(sys.argv) >= 3:
			path_to_unpack = sys.argv[2]
		else:
			path_to_unpack = ''

		if len(sys.argv) >= 4 and sys.argv[3] == '--select':
			select_file = True
	
	else:
		path_to_build = sys.argv[1];
		preset_used = False


	if upload:
		upload_build(path_to_build, preset_used);
	else:
		download_build(path_to_unpack, select_file)

	

	