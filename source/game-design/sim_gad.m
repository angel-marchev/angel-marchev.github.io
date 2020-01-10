%%init
clear all
tic
sz=256;
colorspace=zeros(sz,sz,sz);
unfold=zeros(sz^3,4,10);
for q=1:10
parfor i=1:sz
    for j=1:sz
        for k=1:sz
            [~,~,colorspace(i,j,k)]=gadove(i,j,k,sz);
            [i,j,k,q]
        end
    end
end
toc
            
color_space=(5-colorspace);

for i=1:sz
    for j=1:sz
        for k=1:sz
            s=k+(j-1)*sz+(i-1)*sz*sz;
            unfold(s,1,q)=i;
            unfold(s,2,q)=j;
            unfold(s,3,q)=k;
            if color_space(i,j,k)>0
                unfold(s,4,q)=color_space(i,j,k);
            else
                unfold(s,4,q)=nan;
            end
        end
    end
end

end
unfold_mean=nanmean(unfold,3);
%scatter3(unfold(:,1),unfold(:,2),unfold(:,3),unfold(:,4),unfold(:,4))
unfold_sum=nansum(unfold,3);

figure1 = figure;
axes1 = axes('Parent',figure1);
hold(axes1,'on');
area(unfold_sum(:,4),'EdgeColor','none');
xlim(axes1,[0 size(unfold,1)]);
box(axes1,'on');

mean_color_space=squeeze(nanmean(color_space,1));
%mean_color_space=squeeze(nanmean(color_space,2));
%mean_color_space=squeeze(nanmean(color_space,3));
histogram(mean_color_space)
mesh(mean_color_space)
var_color_space=squeeze(nanmean(color_space,2));
mesh(var_color_space)
