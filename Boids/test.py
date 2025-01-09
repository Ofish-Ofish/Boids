import numpy as np
from pprint import pprint
import math
num_pts = 200
indices = np.arange(0, num_pts, dtype=float) + 0.5


phi = np.arccos(1 - 2*indices/num_pts)
theta = np.pi * (1 + 5**0.5) * indices
# print(phi)
# print(theta)

x, y, z = np.cos(theta) * np.sin(phi), np.sin(theta) * np.sin(phi), np.cos(phi)
print(x[0])
print(y[0])
print(z[0])