/*
 * gadove.cpp
 *
 * Code generation for function 'gadove'
 *
 */

/* Include files */
#include <cmath>
#include <string.h>
#include "rt_nonfinite.h"
#include "gadove.h"
#include "sortLE.h"
#include "rand.h"
#include "gadove_emxutil.h"
#include "sortedInsertion.h"

/* Function Declarations */
static double rt_roundd_snf(double u);

/* Function Definitions */
static double rt_roundd_snf(double u)
{
  double y;
  if (std::abs(u) < 4.503599627370496E+15) {
    if (u >= 0.5) {
      y = std::floor(u + 0.5);
    } else if (u > -0.5) {
      y = u * 0.0;
    } else {
      y = std::ceil(u - 0.5);
    }
  } else {
    y = u;
  }

  return y;
}

void gadove(double x, double y, double z, emxArray_real_T *rec, double best[3],
            double *iter)
{
  double kill[45];
  int k;
  int i0;
  emxArray_real_T *b_rec;
  int i;
  double b_kill;
  double mutate[3];
  int j;
  int idx[5];
  double b[5];
  double eucl[15];
  signed char ind[45];
  int i1;
  int b_idx[15];
  int i2;
  int pEnd;
  int p;
  int q;
  int qEnd;
  int kEnd;
  int iwork[15];
  boolean_T b_x[3];
  boolean_T b_y;
  boolean_T exitg1;

  /*  initialize */
  b_rand(kill);
  for (k = 0; k < 45; k++) {
    kill[k] = rt_roundd_snf(kill[k] * 256.0) - 1.0;
  }

  /* objective=round(rand(1,3)*256,0)-1; */
  *iter = 0.0;
  best[0] = kill[0];
  best[1] = kill[15];
  best[2] = kill[30];
  i0 = rec->size[0] * rec->size[1];
  rec->size[0] = 1;
  rec->size[1] = 3;
  emxEnsureCapacity_real_T(rec, i0);
  rec->data[0] = 0.0;
  rec->data[1] = 0.0;
  rec->data[2] = 0.0;

  /*  begin selection, set terminal conditions */
  emxInit_real_T(&b_rec, 2);
  while ((std::abs(best[0] - x) > 10.0) || (std::abs(best[1] - y) > 10.0) ||
         (std::abs(best[2] - z) > 10.0)) {
    /* min(eucl)~=0 */
    /*  calculate euclidean distance */
    for (i = 0; i < 15; i++) {
      b_kill = kill[i] - x;
      mutate[0] = b_kill * b_kill;
      b_kill = kill[i + 15] - y;
      mutate[1] = b_kill * b_kill;
      b_kill = kill[i + 30] - z;
      eucl[i] = std::sqrt((mutate[0] + mutate[1]) + b_kill * b_kill);
    }

    /*  histogram */
    /*  select best 5 solutions */
    for (i = 0; i < 5; i++) {
      idx[i] = 0;
      b[i] = 0.0;
    }

    for (j = 0; j < 5; j++) {
      k = j;
      sortedInsertion(eucl[j], j + 1, b, &k, idx);
    }

    for (j = 0; j < 10; j++) {
      k = 5;
      sortedInsertion(eucl[j + 5], j + 6, b, &k, idx);
    }

    for (i = 0; i < 5; i++) {
      b[i] = idx[i];
    }

    best[0] = kill[(int)b[0] - 1];
    best[1] = kill[(int)b[0] + 14];
    best[2] = kill[(int)b[0] + 29];
    for (i0 = 0; i0 < 45; i0++) {
      ind[i0] = 0;
    }

    for (i0 = 0; i0 < 3; i0++) {
      for (i1 = 0; i1 < 5; i1++) {
        ind[((int)b[i1] + 15 * i0) - 1] = 1;
      }
    }

    for (i0 = 0; i0 < 45; i0++) {
      kill[i0] *= (double)ind[i0];
    }

    for (i = 0; i < 15; i++) {
      b_idx[i] = 0;
    }

    for (k = 0; k <= 13; k += 2) {
      if (sortLE(kill, k + 1, k + 2)) {
        b_idx[k] = k + 1;
        b_idx[k + 1] = k + 2;
      } else {
        b_idx[k] = k + 2;
        b_idx[k + 1] = k + 1;
      }
    }

    b_idx[14] = 15;
    i = 2;
    while (i < 15) {
      i2 = i << 1;
      j = 1;
      for (pEnd = 1 + i; pEnd < 16; pEnd = qEnd + i) {
        p = j;
        q = pEnd;
        qEnd = j + i2;
        if (qEnd > 16) {
          qEnd = 16;
        }

        k = 0;
        kEnd = qEnd - j;
        while (k + 1 <= kEnd) {
          i0 = b_idx[q - 1];
          i1 = b_idx[p - 1];
          if (sortLE(kill, i1, i0)) {
            iwork[k] = i1;
            p++;
            if (p == pEnd) {
              while (q < qEnd) {
                k++;
                iwork[k] = b_idx[q - 1];
                q++;
              }
            }
          } else {
            iwork[k] = i0;
            q++;
            if (q == qEnd) {
              while (p < pEnd) {
                k++;
                iwork[k] = b_idx[p - 1];
                p++;
              }
            }
          }

          k++;
        }

        for (k = 0; k < kEnd; k++) {
          b_idx[(j + k) - 1] = iwork[k];
        }

        j = qEnd;
      }

      i = i2;
    }

    for (j = 0; j < 3; j++) {
      for (i = 0; i < 15; i++) {
        eucl[i] = kill[(b_idx[i] + 15 * j) - 1];
      }

      memcpy(&kill[j * 15], &eucl[0], 15U * sizeof(double));
    }

    /*  new generation by full pairwise combination */
    kill[5] = rt_roundd_snf((kill[0] + kill[1]) / 2.0);
    kill[20] = rt_roundd_snf((kill[15] + kill[16]) / 2.0);
    kill[35] = rt_roundd_snf((kill[30] + kill[31]) / 2.0);
    kill[6] = rt_roundd_snf((kill[0] + kill[2]) / 2.0);
    kill[21] = rt_roundd_snf((kill[15] + kill[17]) / 2.0);
    kill[36] = rt_roundd_snf((kill[30] + kill[32]) / 2.0);
    kill[7] = rt_roundd_snf((kill[0] + kill[3]) / 2.0);
    kill[22] = rt_roundd_snf((kill[15] + kill[18]) / 2.0);
    kill[37] = rt_roundd_snf((kill[30] + kill[33]) / 2.0);
    kill[8] = rt_roundd_snf((kill[0] + kill[4]) / 2.0);
    kill[23] = rt_roundd_snf((kill[15] + kill[19]) / 2.0);
    kill[38] = rt_roundd_snf((kill[30] + kill[34]) / 2.0);
    kill[9] = rt_roundd_snf((kill[1] + kill[2]) / 2.0);
    kill[24] = rt_roundd_snf((kill[16] + kill[17]) / 2.0);
    kill[39] = rt_roundd_snf((kill[31] + kill[32]) / 2.0);
    kill[10] = rt_roundd_snf((kill[1] + kill[3]) / 2.0);
    kill[25] = rt_roundd_snf((kill[16] + kill[18]) / 2.0);
    kill[40] = rt_roundd_snf((kill[31] + kill[33]) / 2.0);
    kill[11] = rt_roundd_snf((kill[1] + kill[4]) / 2.0);
    kill[26] = rt_roundd_snf((kill[16] + kill[19]) / 2.0);
    kill[41] = rt_roundd_snf((kill[31] + kill[34]) / 2.0);
    kill[12] = rt_roundd_snf((kill[2] + kill[3]) / 2.0);
    kill[27] = rt_roundd_snf((kill[17] + kill[18]) / 2.0);
    kill[42] = rt_roundd_snf((kill[32] + kill[33]) / 2.0);
    kill[13] = rt_roundd_snf((kill[2] + kill[4]) / 2.0);
    kill[28] = rt_roundd_snf((kill[17] + kill[19]) / 2.0);
    kill[43] = rt_roundd_snf((kill[32] + kill[34]) / 2.0);
    kill[14] = rt_roundd_snf((kill[3] + kill[4]) / 2.0);
    kill[29] = rt_roundd_snf((kill[18] + kill[19]) / 2.0);
    kill[44] = rt_roundd_snf((kill[33] + kill[34]) / 2.0);

    /*  maintanance  */
    (*iter)++;

    /* message=['Best match = ', num2str(best), ', iteration: ', num2str(iter),' , objective =',num2str(objective)]; */
    /* message */
    i0 = b_rec->size[0] * b_rec->size[1];
    b_rec->size[0] = rec->size[0] + 1;
    b_rec->size[1] = 3;
    emxEnsureCapacity_real_T(b_rec, i0);
    for (i0 = 0; i0 < 3; i0++) {
      k = rec->size[0];
      for (i1 = 0; i1 < k; i1++) {
        b_rec->data[i1 + b_rec->size[0] * i0] = rec->data[i1 + rec->size[0] * i0];
      }
    }

    b_rec->data[rec->size[0]] = best[0];
    b_rec->data[rec->size[0] + b_rec->size[0]] = best[1];
    b_rec->data[rec->size[0] + (b_rec->size[0] << 1)] = best[2];
    i0 = rec->size[0] * rec->size[1];
    rec->size[0] = b_rec->size[0];
    rec->size[1] = 3;
    emxEnsureCapacity_real_T(rec, i0);
    k = b_rec->size[0] * b_rec->size[1];
    for (i0 = 0; i0 < k; i0++) {
      rec->data[i0] = b_rec->data[i0];
    }

    /*  mutation, if stuck in same solution  */
    k = (int)(unsigned int)*iter;
    b_x[0] = (rec->data[k] == rec->data[(int)*iter - 1]);
    b_x[1] = (rec->data[k + rec->size[0]] == rec->data[((int)*iter + rec->size[0])
              - 1]);
    b_x[2] = (rec->data[k + (rec->size[0] << 1)] == rec->data[((int)*iter +
               (rec->size[0] << 1)) - 1]);
    b_y = true;
    k = 0;
    exitg1 = false;
    while ((!exitg1) && (k < 3)) {
      if (!b_x[k]) {
        b_y = false;
        exitg1 = true;
      } else {
        k++;
      }
    }

    if (b_y) {
      c_rand(mutate);
      mutate[0] = mutate[0] * 30.0 - 15.0;
      mutate[1] = mutate[1] * 30.0 - 15.0;
      mutate[2] = mutate[2] * 30.0 - 15.0;
      for (i = 0; i < 15; i++) {
        kill[i] = rt_roundd_snf(kill[i] + mutate[0]);
        kill[i + 15] = rt_roundd_snf(kill[i + 15] + mutate[1]);
        kill[i + 30] = rt_roundd_snf(kill[i + 30] + mutate[2]);
      }
    }

    /*  capping, if any is <0, >255 */
    for (i = 0; i < 45; i++) {
      b_kill = kill[i];
      if (kill[i] < 0.0) {
        b_kill = 0.0;
      } else {
        if (kill[i] > 255.0) {
          b_kill = 255.0;
        }
      }

      kill[i] = b_kill;
    }
  }

  emxFree_real_T(&b_rec);
}

/* End of code generation (gadove.cpp) */
